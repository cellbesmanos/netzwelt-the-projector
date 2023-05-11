$(document).ready(function () {
  initializeAddProjectMembers();
  initializeRemoveProjectMembers();
  initializeGetProjectMembersListForm();
});

function initializeAddProjectMembers() {
  const addProjectMembersForm = $("form#add-project-members-form")[0];
  const projectMembersListForm = $("form[data-members-list-form]")[0];
  const addURL = $(addProjectMembersForm).data("url");

  $(addProjectMembersForm).on("submit", function (e) {
    e.preventDefault();

    const addSubmitBtn = $(this).find("button[type='submit']");
    $(addSubmitBtn).attr("disabled", true);

    const addProjectMemberForm = new FormData(this);
    const projectId = addProjectMemberForm.get("ProjectId");
    const userId = addProjectMemberForm.get("UserId");

    const projectMembersListFormData = new FormData(projectMembersListForm);
    const search = projectMembersListFormData.get("search");
    const sort = projectMembersListFormData.get("sort");
    const pageNumber = projectMembersListFormData.get("pageNumber");

    localStorage.setItem("search", search);
    localStorage.setItem("sort", sort);
    localStorage.setItem("pageNumber", pageNumber);

    $.ajax({
      url: addURL,
      type: "POST",
      data: JSON.stringify({
        ProjectId: projectId,
        UserId: userId,
      }),
      contentType: "application/json",
    })
      .done(function (data) {
        const projectsMembersContainer = $("div#project-members-container");

        projectsMembersContainer.html(data);
        $("input[type='text'][data-members-list-search-field]").val(
          localStorage.getItem("search")
        );

        const sort = localStorage.getItem("sort");
        $(`input[type='radio'][data-members-list-sort][value='${sort}']`).attr(
          "checked",
          true
        );

        const pageNumber = localStorage.getItem("pageNumber");
        $("select[data-members-list-pagination]")
          .children(`option[value="${pageNumber}"]`)
          .attr("selected", true);

        localStorage.clear();

        initializeAddProjectMembers();
        initializeRemoveProjectMembers();
        initializeGetProjectMembersListForm();
      })
      .fail(function (err) {
        console.log(err);
      })
      .always(function () {
        $(addSubmitBtn).attr("disabled", false);
      });
  });
}

function initializeRemoveProjectMembers() {
  const projectMembersList = $("ul#project-members-list");
  const listURL = $(projectMembersList).data("url");
  const removeButtons = $("li[data-project-member]>button");

  removeButtons.each(function (_, removeButton) {
    $(removeButton).click(function () {
      const projectId = $(this).data("project-id");
      const userId = $(this).data("user-id");

      const membersListForm = $("form[data-members-list-form]")[0];
      const membersListFormData = new FormData(membersListForm);
      const search = membersListFormData.get("search");
      const sort = membersListFormData.get("sort");
      const pageNumber = membersListFormData.get("pageNumber");

      localStorage.setItem("search", search);
      localStorage.setItem("sort", sort);
      localStorage.setItem("pageNumber", pageNumber);

      $(this).attr("disabled", true);

      $.ajax({
        url: listURL,
        type: "POST",
        data: JSON.stringify({
          ProjectId: projectId,
          UserId: userId,
        }),
        contentType: "application/json",
      })
        .done(function (data) {
          const projectsMembersContainer = $("div#project-members-container");

          projectsMembersContainer.html(data);
          $("input[type='text'][data-members-list-search-field]").val(
            localStorage.getItem("search")
          );

          const sort = localStorage.getItem("sort");
          $(
            `input[type='radio'][data-members-list-sort][value='${sort}']`
          ).attr("checked", true);

          const pageNumber = localStorage.getItem("pageNumber");
          $("select[data-members-list-pagination]")
            .children(`option[value="${pageNumber}"]`)
            .attr("selected", true);

          localStorage.clear();

          initializeAddProjectMembers();
          initializeRemoveProjectMembers();
          initializeGetProjectMembersListForm();
        })
        .fail(function (err) {
          console.log(err);
        })
        .always(function () {
          $(removeButton).attr("disabled", false);
        });
    });
  });
}

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
    PageNumber: Number(pageNumber),
    PageSize: 10,
  };

  $.ajax({
    url,
    type: "GET",
    data: GetProjectMembersListQueryParams,
  })
    .done(function (data) {
      container.html(data);

      initializeAddProjectMembers();
      initializeRemoveProjectMembers();
      initializeGetProjectMembersListForm();
    })
    .fail(function (err) {
      console.error(err);
    });
}
