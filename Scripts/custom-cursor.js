// Scripts/custom-cursor.js

// Lấy phần tử cursor từ HTML
const cursor = document.getElementById("cursor");

// Các hằng số cài đặt
const amount = 20; // Số lượng chấm tròn
const width = 26;  // Kích thước của mỗi chấm
const idleTimeout = 150; // Thời gian chờ trước khi chuyển sang trạng thái "nghỉ"

// Các biến trạng thái
let lastFrame = 0;
let mousePosition = { x: 0, y: 0 };
let dots = [];
let timeoutID;
let idle = false;

// Lớp (Class) định nghĩa mỗi chấm tròn
class Dot {
    constructor(index = 0) {
        this.index = index;
        this.anglespeed = 0.05;
        this.x = 0;
        this.y = 0;
        this.scale = 1 - 0.05 * index;
        this.range = width / 2 - (width / 2) * this.scale + 2;
        this.element = document.createElement("span");
        TweenMax.set(this.element, { scale: this.scale });
        cursor.appendChild(this.element);
    }

    lock() {
        this.lockX = this.x;
        this.lockY = this.y;
        this.angleX = Math.PI * 2 * Math.random();
        this.angleY = Math.PI * 2 * Math.random();
    }

    draw(delta) {
        // Chỉ vẽ khi chuột đang di chuyển
        if (!idle) {
            TweenMax.set(this.element, { x: this.x, y: this.y });
        } else {
            // Hiệu ứng "khiêu vũ" khi chuột đứng yên
            this.angleX += this.anglespeed;
            this.angleY += this.anglespeed;
            this.y = this.lockY + Math.sin(this.angleY) * this.range;
            this.x = this.lockX + Math.sin(this.angleX) * this.range;
            TweenMax.set(this.element, { x: this.x, y: this.y });
        }
    }
}

// Hàm khởi tạo
function init() {
    // Lắng nghe sự kiện di chuyển chuột
    window.addEventListener("mousemove", onMouseMove);
    window.addEventListener("touchmove", onTouchMove);

    // Lắng nghe sự kiện di chuột vào/ra các link để phóng to/thu nhỏ
    const navLinks = document.querySelectorAll("a, button, .cta-button, .product-card-link");
    navLinks.forEach(link => {
        link.addEventListener("mouseenter", onMouseEnter);
        link.addEventListener("mouseleave", onMouseLeave);
    });

    // Tạo ra các chấm tròn
    buildDots();
    // Bắt đầu vòng lặp animation
    render();
    // Bắt đầu hẹn giờ cho trạng thái "nghỉ"
    startIdleTimer();
}

// === CÁC HÀM XỬ LÝ SỰ KIỆN ===
function onMouseMove(event) {
    // Cập nhật tọa độ chuột, trừ đi một nửa kích thước để con trỏ nằm ngay tâm
    mousePosition.x = event.clientX;
    mousePosition.y = event.clientY;
    resetIdleTimer();
};

function onTouchMove(event) {
    mousePosition.x = event.touches[0].clientX;
    mousePosition.y = event.touches[0].clientY;
    resetIdleTimer();
};

function onMouseEnter() {
    TweenMax.to(cursor, 0.2, { scale: 1.3 });
}

function onMouseLeave() {
    TweenMax.to(cursor, 0.2, { scale: 1.0 });
}

// === CÁC HÀM ĐIỀU KHIỂN ANIMATION VÀ TRẠNG THÁI ===
function render(timestamp) {
    const delta = timestamp - lastFrame;
    positionCursor(delta);
    lastFrame = timestamp;
    requestAnimationFrame(render);
};

function positionCursor(delta) {
    let x = mousePosition.x;
    let y = mousePosition.y;
    dots.forEach((dot, index, dots) => {
        let nextDot = dots[index + 1] || dots[0];
        dot.x = x;
        dot.y = y;
        dot.draw(delta);
        if (!idle) {
            const dx = (nextDot.x - dot.x) * 0.35;
            const dy = (nextDot.y - dot.y) * 0.35;
            x += dx;
            y += dy;
        }
    });
};

function buildDots() {
    for (let i = 0; i < amount; i++) {
        let dot = new Dot(i);
        dots.push(dot);
    }
}

// === CÁC HÀM XỬ LÝ TRẠNG THÁI "NGHỈ" (IDLE) ===
function startIdleTimer() {
    timeoutID = setTimeout(goInactive, idleTimeout);
    idle = false;
}

function resetIdleTimer() {
    clearTimeout(timeoutID);
    startIdleTimer();
}

function goInactive() {
    idle = true;
    for (let dot of dots) {
        dot.lock();
    }
}

// Bắt đầu tất cả
init();