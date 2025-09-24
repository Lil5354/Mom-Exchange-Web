// Scripts/trade-popup.js

document.addEventListener("DOMContentLoaded", function () {
    const tradePopupOverlay = document.getElementById('trade-popup-overlay');
    const closePopupBtn = document.getElementById('close-popup-btn');
    const cancelPopupBtn = document.getElementById('cancel-popup-btn');
    const sendOfferBtn = document.getElementById('send-offer-btn');
    const tradeOfferButtons = document.querySelectorAll('.trade-card-footer .cta-button');

    let selectedUserItemId = null;

    // --- Hàm Mở/Đóng Popup ---
    function openPopup() {
        tradePopupOverlay.classList.add('visible');
    }
    function closePopup() {
        tradePopupOverlay.classList.remove('visible');
        resetPopup();
    }

    // --- Hàm reset lại popup về trạng thái ban đầu ---
    function resetPopup() {
        document.getElementById('your-item-list').innerHTML = '<p>Đang tải...</p>';
        document.getElementById('item-placeholder').style.display = 'flex';
        document.getElementById('popup-your-item-img').style.display = 'none';
        document.getElementById('popup-your-item-name').textContent = '';
        sendOfferBtn.disabled = true;
        selectedUserItemId = null;
    }

    // --- Gắn sự kiện cho các nút "Đề nghị trao đổi" ---
    tradeOfferButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault();

            // Lấy thông tin từ thẻ cha gần nhất
            const card = this.closest('.trade-card');
            const sellerName = card.querySelector('.seller-info span').textContent.split('-')[0].trim();
            const ownerItemName = card.querySelector('.trade-item-name').textContent;
            const ownerItemImg = card.querySelector('.trade-item-image img').src;

            // Điền thông tin vào popup
            document.getElementById('popup-seller-name').textContent = sellerName;
            document.getElementById('popup-seller-name-2').textContent = sellerName;
            document.getElementById('popup-owner-item-name').textContent = ownerItemName;
            document.getElementById('popup-owner-item-img').src = ownerItemImg;

            // Mở popup
            openPopup();
            // Tải sản phẩm của người dùng
            loadUserProducts();
        });
    });

    // --- Hàm tải sản phẩm của người dùng từ Controller ---
    function loadUserProducts() {
        fetch('/Trade/GetUserProductsForTrade') // Gọi đến Action trong TradeController
            .then(response => response.json())
            .then(data => {
                const itemList = document.getElementById('your-item-list');
                itemList.innerHTML = ''; // Xóa chữ "Đang tải"
                data.forEach(item => {
                    const itemDiv = document.createElement('div');
                    itemDiv.className = 'user-product-item';
                    itemDiv.dataset.itemId = item.Id;
                    itemDiv.dataset.itemName = item.Name;
                    itemDiv.dataset.itemImg = item.ImageUrls[0];
                    itemDiv.innerHTML = `
                        <img src="${item.ImageUrls[0]}" alt="${item.Name}">
                        <p>${item.Name}</p>
                    `;
                    // Gắn sự kiện click để chọn sản phẩm
                    itemDiv.addEventListener('click', function () {
                        // Bỏ chọn tất cả các item khác
                        document.querySelectorAll('.user-product-item').forEach(el => el.classList.remove('selected'));
                        // Chọn item này
                        this.classList.add('selected');
                        selectedUserItemId = this.dataset.itemId;

                        // Hiển thị item đã chọn
                        document.getElementById('item-placeholder').style.display = 'none';
                        const yourImg = document.getElementById('popup-your-item-img');
                        yourImg.src = this.dataset.itemImg;
                        yourImg.style.display = 'block';
                        document.getElementById('popup-your-item-name').textContent = this.dataset.itemName;

                        // Bật nút Gửi đề nghị
                        sendOfferBtn.disabled = false;
                    });

                    itemList.appendChild(itemDiv);
                });
            });
    }

    // --- Gắn sự kiện đóng popup ---
    closePopupBtn.addEventListener('click', closePopup);
    cancelPopupBtn.addEventListener('click', closePopup);
    tradePopupOverlay.addEventListener('click', function (event) {
        if (event.target === tradePopupOverlay) {
            closePopup();
        }
    });

    // --- Sự kiện gửi đề nghị (hiện tại chỉ đóng popup và thông báo) ---
    sendOfferBtn.addEventListener('click', function () {
        if (selectedUserItemId) {
            alert(`Đã gửi đề nghị trao đổi (Sản phẩm ID: ${selectedUserItemId})!`);
            closePopup();
        }
    });
});