const dropZone = document.getElementById("drop-zone");
const fileInput = document.getElementById("file-upload");
const fileList = document.getElementById("file-list");
const form = document.getElementById("upload-pdf-form");

let uploadedFiles = [];

function updateFileList(files) {
  fileList.innerHTML = "";
  files.forEach((file) => {
    const li = document.createElement("li");
    li.textContent = file.name;
    fileList.appendChild(li);
  });
}

function updateFileInput() {
  updateFileList(uploadedFiles);

  const dataTransfer = new DataTransfer();
  uploadedFiles.forEach((file) => dataTransfer.items.add(file));
  fileInput.files = dataTransfer.files;
}

dropZone.addEventListener("dragover", (event) => {
  event.preventDefault();
  dropZone.classList.add("border-primary");
});

dropZone.addEventListener("dragleave", () => {
  dropZone.classList.remove("border-primary");
});

// Handle drop event
dropZone.addEventListener("drop", (event) => {
  event.preventDefault();
  dropZone.classList.remove("border-primary");

  const newFiles = Array.from(event.dataTransfer.files);
  uploadedFiles = [...uploadedFiles, ...newFiles];
  updateFileInput();
});

// Handle manual file input
fileInput.addEventListener("change", () => {
  const newFiles = Array.from(fileInput.files);
  uploadedFiles = [...uploadedFiles, ...newFiles];
  updateFileInput();
});
