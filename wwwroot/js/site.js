// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(() => {
  const postForms = $("form[data-post-form]");

  postForms.each((_, form) => {
    const submitButton = $(postForms).children("button[type='submit']")[0];

    $(form).submit(() => {
      $(submitButton).prop("disabled", true);

      if (!$(form).valid()) {
        $(submitButton).prop("disabled", false);
      }
    });

    const fieldsToValidate = $(form).find("input[data-val='true'");

    fieldsToValidate.each((_, field) => {
      $(field).one("input", () => {
        $(form).validate().element(field);
      });
    });
  });

  const popupShowButtons = $("button[data-pop-up-show]");
  const popupHideButtons = $("button[data-pop-up-hide]");

  $(popupShowButtons).each((_, b) => {
    $(b).click((e) => {
      const popupTargetName = $(e.target).data("pop-up-show");
      const popupTargetElem = $(`div[data-pop-up='${popupTargetName}']`);

      $(popupTargetElem)
        .children("div")
        .click((e) => {
          e.stopPropagation();
        });

      $(popupTargetElem).click(function () {
        $(this).hide();
      });

      popupTargetElem.show();
    });
  });
  $(popupHideButtons).each((_, b) => {
    $(b).click((e) => {
      const popupTargetName = $(e.target).data("pop-up-hide");
      const popupTargetElem = $(`div[data-pop-up='${popupTargetName}']`);

      popupTargetElem.hide();
    });
  });
});
