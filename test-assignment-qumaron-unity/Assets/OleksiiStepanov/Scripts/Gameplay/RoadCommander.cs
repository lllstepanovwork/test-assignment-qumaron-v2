using System.Collections.Generic;
using System;
using System.Linq;

namespace OleksiiStepanov.Gameplay
{
    public class RoadCommander
    {
        private bool _undoStatus;
        private bool _redoStatus;

        public static event Action<bool> OnUndoStatusChanged; 
        public static event Action<bool> OnRedoStatusChanged; 
        
        private readonly List<IBuildRoadCommand> _roadCommands = new List<IBuildRoadCommand>();
        
        public void CreateNewBuildCommand(GridElement gridElement, RoadSO roadSo)
        {
            RemoveAllDeactivatedRoadCommands();
            
            IBuildRoadCommand buildRoadCommand = new BuildRoadCommand(gridElement, roadSo); 
            
            _roadCommands.Add(buildRoadCommand);
            
            buildRoadCommand.Execute();
            
            UpdateUndoStatus();
            UpdateRedoStatus();
        }

        public void Undo(Action<GridElement> onComplete = null)
        {
            if (_roadCommands.Count == 0) return;
            
            IBuildRoadCommand lastBuildRoadCommand = GetLastActiveRoadCommand();
            
            if (lastBuildRoadCommand == null) return;
            
            lastBuildRoadCommand.Undo();

            onComplete?.Invoke(lastBuildRoadCommand.GetGridElement());
            
            UpdateUndoStatus();
            UpdateRedoStatus();
        }
        
        public void Redo(Action<GridElement> onComplete = null)
        {
            if (_roadCommands.Count == 0) return;

            foreach (IBuildRoadCommand buildRoadCommand in _roadCommands)
            {
                if (!buildRoadCommand.Active)
                {
                    buildRoadCommand.Execute();

                    onComplete?.Invoke(buildRoadCommand.GetGridElement());

                    break;
                }
            }
            
            UpdateUndoStatus();
            UpdateRedoStatus();
        }

        private IBuildRoadCommand GetLastActiveRoadCommand()
        {
            for (int i = _roadCommands.Count - 1; i >= 0 ; i--)
            {
                if (_roadCommands[i].Active) return _roadCommands[i];
            }

            return null;
        }

        private void RemoveAllDeactivatedRoadCommands()
        {
            foreach (var roadCommand in _roadCommands.ToList())
            {
                if (!roadCommand.Active)
                {
                    _roadCommands.Remove(roadCommand);
                }
            }
        }

        public void ResetAll()
        {
            _roadCommands.Clear();
            
            UpdateRedoStatus();
            UpdateUndoStatus();
        }

        private void UpdateRedoStatus()
        {
            _redoStatus = false;
            
            foreach (var roadCommand in _roadCommands)
            {
                if (roadCommand.Active == false)
                {
                    _redoStatus = true;
                    
                    break;
                }
            }
            
            OnRedoStatusChanged?.Invoke(_redoStatus);
        }

        private void UpdateUndoStatus()
        {
            _undoStatus = false;
            
            foreach (var roadCommand in _roadCommands)
            {
                if (roadCommand.Active)
                {
                    _undoStatus = true;
                    
                    break;
                }
            }
            
            OnUndoStatusChanged?.Invoke(_undoStatus);
        }
        
        public void Confirm()
        {
            foreach (var roadCommand in _roadCommands)
            {
                roadCommand.Confirm();
            }

            _roadCommands.Clear();
        }
    }
    
    public class BuildRoadCommand : IBuildRoadCommand
    {
        public bool Active { get; set; }
        
        private readonly GridElement _gridElement;
        private readonly RoadSO _roadSo;

        public BuildRoadCommand(GridElement gridElement, RoadSO roadSo)
        {
            _gridElement = gridElement;
            _roadSo = roadSo;
        }

        public void Execute() 
        {
            Active = true;
            
            _gridElement.OccupyByRoad(_roadSo);
        }

        public void Confirm()
        {
            _gridElement.ConfirmRoadOccupation();
        }

        public void Undo()
        {
            Active = false;
            
            _gridElement.ResetRoadOccupation();
        }

        public GridElement GetGridElement()
        {
            return _gridElement;
        }
    }
    
    public interface IBuildRoadCommand
    {
        public bool Active { get; set; }

        public void Execute();
        public void Confirm();
        public void Undo();
        
        public GridElement GetGridElement();
    }
}
