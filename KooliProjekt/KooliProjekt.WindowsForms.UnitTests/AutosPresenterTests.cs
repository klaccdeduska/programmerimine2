using KooliProjekt.WindowsForms.Api;
using KooliProjekt.WindowsForms.Models;
using KooliProjekt.WindowsForms.Presenter;
using KooliProjekt.WindowsForms.View;
using Moq;
using Xunit;

namespace KooliProjekt.WindowsForms.UnitTests
{
    public class AutosPresenterTests
    {
        private static OperationResult<T> ErrorResult<T>(string error = "Test error")
        {
            var result = new OperationResult<T>();
            result.Errors.Add(error);
            return result;
        }

        private static OperationResult<T> SuccessResult<T>(T value)
        {
            return new OperationResult<T>
            {
                Value = value
            };
        }

        [Fact]
        public async Task LoadData_should_call_ShowError_with_faulty_response()
        {
            var view = new Mock<IAutosView>();
            var apiClient = new Mock<IApiClient>();

            var errorResult = ErrorResult<List<AutoModel>>();

            apiClient
                .Setup(x => x.GetAutosAsync())
                .ReturnsAsync(errorResult);

            var presenter = new AutosPresenter(view.Object, apiClient.Object);

            await presenter.LoadDataAsync();

            view.Verify(x => x.ShowError(errorResult), Times.Once);
            view.VerifySet(x => x.Autos = It.IsAny<IList<AutoModel>>(), Times.Never);
        }

        [Fact]
        public async Task LoadData_should_set_DataSource_with_valid_response()
        {
            var view = new Mock<IAutosView>();
            var apiClient = new Mock<IApiClient>();

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
                    Mudel = "X5",
                    Numbrimark = "456DEF"
                }
            };

            apiClient
                .Setup(x => x.GetAutosAsync())
                .ReturnsAsync(SuccessResult(autos));

            var presenter = new AutosPresenter(view.Object, apiClient.Object);

            await presenter.LoadDataAsync();

            view.VerifySet(x => x.Autos = It.Is<IList<AutoModel>>(list =>
                list.Count == 2 &&
                list[0].Tootja == "Toyota" &&
                list[1].Tootja == "BMW"), Times.Once);

            view.Verify(x => x.ShowError(It.IsAny<OperationResult>()), Times.Never);
        }

        [Fact]
        public void SetSelection_should_clear_fields_with_null_selection()
        {
            var view = new Mock<IAutosView>();
            var apiClient = new Mock<IApiClient>();

            var presenter = new AutosPresenter(view.Object, apiClient.Object);

            presenter.SetSelection(null);

            view.VerifySet(x => x.CurrentId = 0, Times.Once);
            view.VerifySet(x => x.CurrentTootja = "", Times.Once);
            view.VerifySet(x => x.CurrentMudel = "", Times.Once);
            view.VerifySet(x => x.CurrentNumbrimark = "", Times.Once);
        }

        [Fact]
        public void SetSelection_should_set_fields_with_valid_selection()
        {
            var view = new Mock<IAutosView>();
            var apiClient = new Mock<IApiClient>();

            var presenter = new AutosPresenter(view.Object, apiClient.Object);

            var auto = new AutoModel
            {
                Id = 5,
                Tootja = "Honda",
                Mudel = "Accord",
                Numbrimark = "XYZ123"
            };

            presenter.SetSelection(auto);

            view.VerifySet(x => x.CurrentId = 5, Times.Once);
            view.VerifySet(x => x.CurrentTootja = "Honda", Times.Once);
            view.VerifySet(x => x.CurrentMudel = "Accord", Times.Once);
            view.VerifySet(x => x.CurrentNumbrimark = "XYZ123", Times.Once);
        }

        [Fact]
        public async Task Save_should_call_ShowError_with_faulty_response()
        {
            var view = new Mock<IAutosView>();
            var apiClient = new Mock<IApiClient>();

            view.SetupGet(x => x.CurrentId).Returns(1);
            view.SetupGet(x => x.CurrentTootja).Returns("Toyota");
            view.SetupGet(x => x.CurrentMudel).Returns("Corolla");
            view.SetupGet(x => x.CurrentNumbrimark).Returns("123ABC");

            var errorResult = ErrorResult<AutoModel>();

            apiClient
                .Setup(x => x.SaveAutoAsync(It.IsAny<AutoModel>()))
                .ReturnsAsync(errorResult);

            var presenter = new AutosPresenter(view.Object, apiClient.Object);

            await presenter.Save();

            view.Verify(x => x.ShowError(errorResult), Times.Once);
            apiClient.Verify(x => x.GetAutosAsync(), Times.Never);
        }

        [Fact]
        public async Task Save_should_call_LoadData_with_valid_response()
        {
            var view = new Mock<IAutosView>();
            var apiClient = new Mock<IApiClient>();

            view.SetupGet(x => x.CurrentId).Returns(1);
            view.SetupGet(x => x.CurrentTootja).Returns("Toyota");
            view.SetupGet(x => x.CurrentMudel).Returns("Corolla");
            view.SetupGet(x => x.CurrentNumbrimark).Returns("123ABC");

            var savedAuto = new AutoModel
            {
                Id = 1,
                Tootja = "Toyota",
                Mudel = "Corolla",
                Numbrimark = "123ABC"
            };

            var autos = new List<AutoModel>
            {
                savedAuto
            };

            apiClient
                .Setup(x => x.SaveAutoAsync(It.Is<AutoModel>(a =>
                    a.Id == 1 &&
                    a.Tootja == "Toyota" &&
                    a.Mudel == "Corolla" &&
                    a.Numbrimark == "123ABC")))
                .ReturnsAsync(SuccessResult(savedAuto));

            apiClient
                .Setup(x => x.GetAutosAsync())
                .ReturnsAsync(SuccessResult(autos));

            var presenter = new AutosPresenter(view.Object, apiClient.Object);

            await presenter.Save();

            apiClient.Verify(x => x.SaveAutoAsync(It.IsAny<AutoModel>()), Times.Once);
            apiClient.Verify(x => x.GetAutosAsync(), Times.Once);

            view.VerifySet(x => x.Autos = It.Is<IList<AutoModel>>(list =>
                list.Count == 1 &&
                list[0].Id == 1), Times.Once);

            view.Verify(x => x.ShowError(It.IsAny<OperationResult>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_return_when_user_didnot_confirmed()
        {
            var view = new Mock<IAutosView>();
            var apiClient = new Mock<IApiClient>();

            view.SetupGet(x => x.CurrentId).Returns(10);
            view.Setup(x => x.ConfirmDelete()).Returns(false);

            var presenter = new AutosPresenter(view.Object, apiClient.Object);

            await presenter.Delete();

            view.Verify(x => x.ConfirmDelete(), Times.Once);
            apiClient.Verify(x => x.DeleteAutoAsync(It.IsAny<int>()), Times.Never);
            apiClient.Verify(x => x.GetAutosAsync(), Times.Never);
            view.Verify(x => x.ShowError(It.IsAny<OperationResult>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_call_ShowError_with_faulty_response()
        {
            var view = new Mock<IAutosView>();
            var apiClient = new Mock<IApiClient>();

            view.SetupGet(x => x.CurrentId).Returns(10);
            view.Setup(x => x.ConfirmDelete()).Returns(true);

            var errorResult = ErrorResult<bool>();

            apiClient
                .Setup(x => x.DeleteAutoAsync(10))
                .ReturnsAsync(errorResult);

            var presenter = new AutosPresenter(view.Object, apiClient.Object);

            await presenter.Delete();

            view.Verify(x => x.ConfirmDelete(), Times.Once);
            apiClient.Verify(x => x.DeleteAutoAsync(10), Times.Once);
            view.Verify(x => x.ShowError(errorResult), Times.Once);
            apiClient.Verify(x => x.GetAutosAsync(), Times.Never);
        }
    }
}