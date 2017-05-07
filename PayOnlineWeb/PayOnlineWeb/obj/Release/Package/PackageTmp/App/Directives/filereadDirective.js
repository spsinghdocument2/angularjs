//your directive
app.directive("fileread", [
  function () {
      return {
          scope: {
              fileread: "="
          },
          link: function (scope, element, attributes) {
              element.bind("change", function (changeEvent) {
                  var reader = new FileReader();
                  reader.onload = function (loadEvent) {
                      scope.$apply(function () {
                          scope.fileread = loadEvent.target.result;
                      });
                  }
                  var ext = changeEvent.target.files[0].name.match(/\.(.+)$/)[1];
                  if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg' || angular.lowercase(ext) === 'png') {
                      if (changeEvent.target.files[0].size >= 2048000) {
                          $('.image-error').text("File size must be less than 2MB.");
                          $('.image-error').show();
                          $('.image-preview-filename').val(null);
                          element.val(null);
                      }
                      else {
                          $('.image-error').hide();
                          $('.image-preview-filename').val(changeEvent.target.files[0].name);
                          reader.readAsDataURL(changeEvent.target.files[0]);
                      }
                  }
                  else {
                      $('.image-error').text("You can only select .png/.jpg/.jpeg file format.");
                      $('.image-error').show();
                      $('.image-preview-filename').val(null);
                      element.val(null);
                  }
              });
          }
      }
  }
]);