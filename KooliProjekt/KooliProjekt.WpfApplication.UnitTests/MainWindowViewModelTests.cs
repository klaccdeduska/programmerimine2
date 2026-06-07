using KooliProjekt.WpfApplication;
using Moq;
using Xunit;

namespace KooliProjekt.WpfApplication.UnitTests
{
    public class MainWindowViewModelTests
    {
        private static OperationResult<T> SuccessResult<T>(T value)
        {
            return new OperationResult<T>
            {
                Value = value
            };
        }

        private static OperationResult<T> ErrorResult<T>(string error = "Test error")
        {
            var result = new OperationResult<T>();
            result.Errors.Add(error);
            return result;
        }

        private static async Task ExecuteCommandAsync(System.Windows.Input.ICommand command)
        {
            var relayCommand = Assert.IsType<RelayCommand>(command);

            await relayCommand.ExecuteAsync(null);
        }

        [Fact]
        public async Task LoadData_should_call_ShowError_with_faulty_response()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var errorResult = ErrorResult<List<AutoModel>>("API error");

            apiClient
                .Setup(x => x.GetAutosAsync())
                .ReturnsAsync(errorResult);

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object);

            await viewModel.LoadDataAsync();

            dialogProvider.Verify(x => x.ShowError(It.Is<string>(s => s.Contains("API error"))), Times.Once);
            Assert.Empty(viewModel.Autos);
            Assert.Contains("API error", viewModel.ErrorMessage);
        }

        [Fact]
        public async Task LoadData_should_set_DataSource_with_valid_response()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var autos = new List<AutoModel>
            {
                new AutoModel
                {
                    Id = 1,
                    Tootja = "Toyota",
                    Mudel = "Corolla",
                    Numbrimark = "123ABC"
                },
                new AutoModel
                {
                    Id = 2,
                    Tootja = "BMW",
                    Mudel = "320",
                    Numbrimark = "555BMW"
                }
            };

            apiClient
                .Setup(x => x.GetAutosAsync())
                .ReturnsAsync(SuccessResult(autos));

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object);

            await viewModel.LoadDataAsync();

            Assert.Equal(2, viewModel.Autos.Count);
            Assert.Equal("Toyota", viewModel.Autos[0].Tootja);
            Assert.Equal("BMW", viewModel.Autos[1].Tootja);

            Assert.NotNull(viewModel.SelectedAuto);
            Assert.Equal(1, viewModel.CurrentId);
            Assert.Equal("Toyota", viewModel.CurrentTootja);
            Assert.Equal("Corolla", viewModel.CurrentMudel);
            Assert.Equal("123ABC", viewModel.CurrentNumbrimark);

            dialogProvider.Verify(x => x.ShowError(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void SelectedAuto_should_clear_fields_with_null_selection()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object);

            viewModel.SelectedAuto = new AutoModel
            {
                Id = 5,
                Tootja = "Honda",
                Mudel = "Accord",
                Numbrimark = "XYZ123"
            };

            viewModel.SelectedAuto = null;

            Assert.Equal(0, viewModel.CurrentId);
            Assert.Equal("", viewModel.CurrentTootja);
            Assert.Equal("", viewModel.CurrentMudel);
            Assert.Equal("", viewModel.CurrentNumbrimark);
        }

        [Fact]
        public void SelectedAuto_should_set_fields_with_valid_selection()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object);

            var auto = new AutoModel
            {
                Id = 10,
                Tootja = "Audi",
                Mudel = "A6",
                Numbrimark = "AUD123"
            };

            viewModel.SelectedAuto = auto;

            Assert.Equal(10, viewModel.CurrentId);
            Assert.Equal("Audi", viewModel.CurrentTootja);
            Assert.Equal("A6", viewModel.CurrentMudel);
            Assert.Equal("AUD123", viewModel.CurrentNumbrimark);
        }

        [Fact]
        public void Changing_current_fields_should_update_selected_auto()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object);

            var auto = new AutoModel
            {
                Id = 1,
                Tootja = "Old",
                Mudel = "Old",
                Numbrimark = "OLD123"
            };

            viewModel.SelectedAuto = auto;

            viewModel.CurrentTootja = "Mercedes";
            viewModel.CurrentMudel = "E";
            viewModel.CurrentNumbrimark = "MER123";

            Assert.Equal("Mercedes", auto.Tootja);
            Assert.Equal("E", auto.Mudel);
            Assert.Equal("MER123", auto.Numbrimark);
        }

        [Fact]
        public async Task AddNewCommand_should_call_ShowError_with_faulty_response()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var errorResult = ErrorResult<AutoModel>("Add error");

            apiClient
                .Setup(x => x.SaveAutoAsync(It.IsAny<AutoModel>()))
                .ReturnsAsync(errorResult);

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object);

            await ExecuteCommandAsync(viewModel.AddNewCommand);

            apiClient.Verify(x => x.SaveAutoAsync(It.Is<AutoModel>(a => a.Id == 0)), Times.Once);
            dialogProvider.Verify(x => x.ShowError(It.Is<string>(s => s.Contains("Add error"))), Times.Once);
            apiClient.Verify(x => x.GetAutosAsync(), Times.Never);
        }

        [Fact]
        public async Task AddNewCommand_should_call_LoadData_with_valid_response()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var savedAuto = new AutoModel
            {
                Id = 3,
                Tootja = "Uus tootja",
                Mudel = "Uus mudel",
                Numbrimark = "NEW1234"
            };

            var autos = new List<AutoModel>
            {
                savedAuto
            };

            apiClient
                .Setup(x => x.SaveAutoAsync(It.IsAny<AutoModel>()))
                .ReturnsAsync(SuccessResult(savedAuto));

            apiClient
                .Setup(x => x.GetAutosAsync())
                .ReturnsAsync(SuccessResult(autos));

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object);

            await ExecuteCommandAsync(viewModel.AddNewCommand);

            apiClient.Verify(x => x.SaveAutoAsync(It.IsAny<AutoModel>()), Times.Once);
            apiClient.Verify(x => x.GetAutosAsync(), Times.Once);

            Assert.Single(viewModel.Autos);
            Assert.Equal(3, viewModel.Autos[0].Id);

            dialogProvider.Verify(x => x.ShowError(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task SaveCommand_should_call_ShowError_with_faulty_response()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var errorResult = ErrorResult<AutoModel>("Save error");

            apiClient
                .Setup(x => x.SaveAutoAsync(It.IsAny<AutoModel>()))
                .ReturnsAsync(errorResult);

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object)
            {
                CurrentId = 1,
                CurrentTootja = "Toyota",
                CurrentMudel = "Corolla",
                CurrentNumbrimark = "123ABC"
            };

            await ExecuteCommandAsync(viewModel.SaveCommand);

            apiClient.Verify(x => x.SaveAutoAsync(It.Is<AutoModel>(a =>
                a.Id == 1 &&
                a.Tootja == "Toyota" &&
                a.Mudel == "Corolla" &&
                a.Numbrimark == "123ABC")), Times.Once);

            dialogProvider.Verify(x => x.ShowError(It.Is<string>(s => s.Contains("Save error"))), Times.Once);
            apiClient.Verify(x => x.GetAutosAsync(), Times.Never);
        }

        [Fact]
        public async Task SaveCommand_should_call_LoadData_with_valid_response()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var savedAuto = new AutoModel
            {
                Id = 1,
                Tootja = "BMW",
                Mudel = "320",
                Numbrimark = "555BMW"
            };

            var autos = new List<AutoModel>
            {
                savedAuto
            };

            apiClient
                .Setup(x => x.SaveAutoAsync(It.IsAny<AutoModel>()))
                .ReturnsAsync(SuccessResult(savedAuto));

            apiClient
                .Setup(x => x.GetAutosAsync())
                .ReturnsAsync(SuccessResult(autos));

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object)
            {
                CurrentId = 1,
                CurrentTootja = "BMW",
                CurrentMudel = "320",
                CurrentNumbrimark = "555BMW"
            };

            await ExecuteCommandAsync(viewModel.SaveCommand);

            apiClient.Verify(x => x.SaveAutoAsync(It.Is<AutoModel>(a =>
                a.Id == 1 &&
                a.Tootja == "BMW" &&
                a.Mudel == "320" &&
                a.Numbrimark == "555BMW")), Times.Once);

            apiClient.Verify(x => x.GetAutosAsync(), Times.Once);

            Assert.Single(viewModel.Autos);
            Assert.Equal("BMW", viewModel.Autos[0].Tootja);

            dialogProvider.Verify(x => x.ShowError(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCommand_should_show_message_when_no_selection()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object)
            {
                CurrentId = 0,
                SelectedAuto = null
            };

            await ExecuteCommandAsync(viewModel.DeleteCommand);

            dialogProvider.Verify(x => x.ShowMessage(It.Is<string>(s => s.Contains("Vali rida"))), Times.Once);
            dialogProvider.Verify(x => x.ConfirmDelete(It.IsAny<AutoModel>()), Times.Never);
            apiClient.Verify(x => x.DeleteAutoAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCommand_should_return_when_user_didnot_confirmed()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var auto = new AutoModel
            {
                Id = 10,
                Tootja = "Honda",
                Mudel = "Accord",
                Numbrimark = "XYZ123"
            };

            dialogProvider
                .Setup(x => x.ConfirmDelete(auto))
                .Returns(false);

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object)
            {
                SelectedAuto = auto
            };

            await ExecuteCommandAsync(viewModel.DeleteCommand);

            dialogProvider.Verify(x => x.ConfirmDelete(auto), Times.Once);
            apiClient.Verify(x => x.DeleteAutoAsync(It.IsAny<int>()), Times.Never);
            apiClient.Verify(x => x.GetAutosAsync(), Times.Never);
            dialogProvider.Verify(x => x.ShowError(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCommand_should_call_ShowError_with_faulty_response()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var auto = new AutoModel
            {
                Id = 10,
                Tootja = "Honda",
                Mudel = "Accord",
                Numbrimark = "XYZ123"
            };

            dialogProvider
                .Setup(x => x.ConfirmDelete(auto))
                .Returns(true);

            var errorResult = ErrorResult<bool>("Delete error");

            apiClient
                .Setup(x => x.DeleteAutoAsync(10))
                .ReturnsAsync(errorResult);

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object)
            {
                SelectedAuto = auto
            };

            await ExecuteCommandAsync(viewModel.DeleteCommand);

            dialogProvider.Verify(x => x.ConfirmDelete(auto), Times.Once);
            apiClient.Verify(x => x.DeleteAutoAsync(10), Times.Once);
            dialogProvider.Verify(x => x.ShowError(It.Is<string>(s => s.Contains("Delete error"))), Times.Once);
            apiClient.Verify(x => x.GetAutosAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteCommand_should_call_LoadData_with_valid_response()
        {
            var apiClient = new Mock<IApiClient>();
            var dialogProvider = new Mock<IDialogProvider>();

            var auto = new AutoModel
            {
                Id = 10,
                Tootja = "Honda",
                Mudel = "Accord",
                Numbrimark = "XYZ123"
            };

            dialogProvider
                .Setup(x => x.ConfirmDelete(auto))
                .Returns(true);

            apiClient
                .Setup(x => x.DeleteAutoAsync(10))
                .ReturnsAsync(SuccessResult(true));

            apiClient
                .Setup(x => x.GetAutosAsync())
                .ReturnsAsync(SuccessResult(new List<AutoModel>()));

            var viewModel = new MainWindowViewModel(apiClient.Object, dialogProvider.Object)
            {
                SelectedAuto = auto
            };

            await ExecuteCommandAsync(viewModel.DeleteCommand);

            dialogProvider.Verify(x => x.ConfirmDelete(auto), Times.Once);
            apiClient.Verify(x => x.DeleteAutoAsync(10), Times.Once);
            apiClient.Verify(x => x.GetAutosAsync(), Times.Once);

            Assert.Empty(viewModel.Autos);
            Assert.Equal(0, viewModel.CurrentId);
            Assert.Equal("", viewModel.CurrentTootja);
            Assert.Equal("", viewModel.CurrentMudel);
            Assert.Equal("", viewModel.CurrentNumbrimark);

            dialogProvider.Verify(x => x.ShowError(It.IsAny<string>()), Times.Never);
        }
    }
}