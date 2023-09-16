using System.Threading.Tasks;
using App.Scripts.Infrastructure.GameCore.States.SetupState;
using App.Scripts.Infrastructure.LevelSelection;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels.View.ViewGridLetters;
using App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel;

namespace App.Scripts.Scenes.SceneFillwords.States.Setup
{
    public class HandlerSetupFillwords : IHandlerSetupLevel
    {
        private readonly ContainerGrid _containerGrid;
        private readonly IProviderFillwordLevel _providerFillwordLevel;
        private readonly IServiceLevelSelection _serviceLevelSelection;
        private readonly ViewGridLetters _viewGridLetters;

        public HandlerSetupFillwords(IProviderFillwordLevel providerFillwordLevel,
            IServiceLevelSelection serviceLevelSelection,
            ViewGridLetters viewGridLetters, ContainerGrid containerGrid)
        {
            _providerFillwordLevel = providerFillwordLevel;
            _serviceLevelSelection = serviceLevelSelection;
            _viewGridLetters = viewGridLetters;
            _containerGrid = containerGrid;
        }

        public Task Process()
        {
            var model = GetModel();

            _viewGridLetters.UpdateItems(model);
            _containerGrid.SetupGrid(model, _serviceLevelSelection.CurrentLevelIndex);
            return Task.CompletedTask;
        }

        private GridFillWords GetModel()
        {
            var model = _providerFillwordLevel.LoadModel(_serviceLevelSelection.CurrentLevelIndex);
            if (model == null)
            {
                int brokenLevel = _serviceLevelSelection.CurrentLevelIndex;
                int nextLevel = brokenLevel;
                do
                {
                    nextLevel++;
                    _serviceLevelSelection.UpdateSelectedLevel(nextLevel);
                    model = _providerFillwordLevel.LoadModel(nextLevel);
                } while (model == null && nextLevel != brokenLevel);
            }

            return model;
        }
    }
}