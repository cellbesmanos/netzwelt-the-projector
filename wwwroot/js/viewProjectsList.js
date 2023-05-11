$(document).ready(function () {
  initializeGetUserListForm();
});

function initializeGetUserListForm() {
  const searchButton = $("button[type='button'][data-project-list-search]")[0];
  const sortButtons = $("input[type='radio'][data-project-list-sort]");
  const paginationDropdown = $("select[data-project-list-pagination]")[0];

  $(searchButton).click(function () {
    getProjectList();
  });

  sortButtons.each(function (_, sortButton) {
    $(sortButton).click(function () {
      getProjectList();
    });
  });

  $(paginationDropdown).on("change", function () {
    getProjectList();
  });
}

function getProjectList() {
  const listForm = $("form[data-project-list-form]")[0];
  const container = $("div#projects-list-container");
  const url = $(container).data("url");

  console.log(url);

  const formData = new FormData(listForm);
  const search = formData.get("search");
  const sort = formData.get("sort");
  const pageNumber = formData.get("pageNumber");
  const GetProjectsQueryParams = {
    Search: search,
    Sort: sort,
    PageNumber: pageNumber,
    PageSize: 10,
  };
  console.log(GetProjectsQueryParams);

  $.ajax({
    url,
    type: "GET",
    data: GetProjectsQueryParams,
  })
    .done(function (data) {
      container.html(data);
      initializeGetUserListForm();
    })
    .fail(function (err) {
      console.error(err);
    });
}
