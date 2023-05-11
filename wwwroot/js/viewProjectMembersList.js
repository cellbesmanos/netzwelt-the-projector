$(document).ready(function () {
  initializeGetProjectMembersListForm();
});

function initializeGetProjectMembersListForm() {
  const searchButton = $("button[type='button'][data-members-list-search]");
  const sortButtons = $("input[type='radio'][data-members-list-sort]");
  const paginationDropdown = $("select[data-members-list-pagination]")[0];

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
  const listForm = $("form[data-members-list-form]")[0];
  const container = $("div#project-members-list-container");
  const url = $(container).data("url");

  const formData = new FormData(listForm);
  const search = formData.get("search");
  const sort = formData.get("sort");
  const pageNumber = formData.get("pageNumber");
  const GetProjectMembersListQueryParams = {
    Search: search,
    Sort: sort,
    PageNumber: pageNumber,
    PageSize: 10,
  };
  console.log(GetProjectMembersListQueryParams);

  $.ajax({
    url,
    type: "GET",
    data: GetProjectMembersListQueryParams,
  })
    .done(function (data) {
      container.html(data);
      initializeGetProjectMembersListForm();
    })
    .fail(function (err) {
      console.error(err);
    });
}
