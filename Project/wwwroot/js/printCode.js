function printQRCodes() {
  const quantity = document.getElementById("qr-quantity").value;

  const room = "@Model.Room";
  const id = "@Model.Id";

  const qrCodeUrl = `/machine/qrcode/${room}/${id}`;
  const printWindow = window.open("", "_blank");
  printWindow.document.write(
    '<html><head><title>Print QR Codes</title></head><body><div class="grid-container">'
  );
  printWindow.document.write(`
                                                        <style>
                                                        .grid-container {
                                                            display: grid;
                                                            grid-template-columns: repeat(auto-fill, minmax(8cm, 1fr));
                                                            gap: 10px;
                                                        }

                                                        .grid-item {
                                                            padding: 5px;
                                                        }

                                                        .grid-item img {
                                                            width: 100%;
                                                            height: auto;
                                                        }
                                                        </style>`);
  for (let i = 0; i < quantity; i++) {
    printWindow.document.write(
      '<div class="qr-code grid-item"><img src="' +
        qrCodeUrl +
        '" alt="QR Code" /></div>'
    );
  }

  printWindow.document.write("</div></body></html>");

  printWindow.print();
  printWindow.close();
}
