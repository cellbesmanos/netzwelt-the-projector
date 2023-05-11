$(document).ready(function () {
  initializeGetUserListForm();
});

function initializeGetUserListForm() {
  const searchButton = $("button[type='button'][data-user-list-search]")[0];
  const filtersDropdown = $("select[data-user-list-filter]")[0];
  const sortButtons = $("input[type='radio'][data-user-list-sort]");
  const paginationDropdown = $("select[data-user-list-pagination]")[0];

  $(searchButton).click(function () {
    getUserList();
  });

  $(filtersDropdown).on("change", function () {
    getUserList();
  });

  sortButtons.each(function (_, sortButton) {
    $(sortButton).click(function () {
      getUserList();
    });
  });

  $(paginationDropdown).on("change", function () {
    getUserList();
  });
}

function getUserList() {
  const listForm = $("form[data-user-list-form]")[0];
  const container = $("div#users-list-container");
  const url = $(container).data("url");

  const formData = new FormData(listForm);
  const search = formData.get("search");
  const filter = formData.get("filter");
  const sort = formData.get("sort");
  const pageNumber = formData.get("pageNumber");
  const GetUsersQueryParams = {
    Search: search,
    FilterBy: filter,
    Sort: sort,
    PageNumber: pageNumber,
    PageSize: 10,
  };

  console.log(GetUsersQueryParams);

  $.ajax({
    url,
    type: "GET",
    data: GetUsersQueryParams,
  })
    .done(function (data) {
      container.html(data);
      initializeGetUserListForm();
    })
    .fail(function (err) {
      console.error(err);
    });
}
