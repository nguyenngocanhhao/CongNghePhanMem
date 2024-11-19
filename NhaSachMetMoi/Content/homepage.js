 
document.addEventListener("DOMContentLoaded", () => {
    const slides = document.querySelectorAll('.slider-item');
    const prevBtn = document.querySelector('.prev-btn');
    const nextBtn = document.querySelector('.next-btn');
    let currentIndex = 0;

    function showSlide(index) {
        slides.forEach((slide, i) => {
            slide.classList.toggle('active', i === index);
        });
    }

    function nextSlide() {
        currentIndex = (currentIndex + 1) % slides.length;
        showSlide(currentIndex);
    }

    function prevSlide() {
        currentIndex = (currentIndex - 1 + slides.length) % slides.length;
        showSlide(currentIndex);
    }

    nextBtn.addEventListener('click', nextSlide);
    prevBtn.addEventListener('click', prevSlide);

    showSlide(currentIndex); // Hiển thị slide đầu tiên
});


document.addEventListener("DOMContentLoaded", () => {
    const bgText = document.querySelector('.background-text');
    let position = -bgText.clientWidth; // Bắt đầu vị trí ở ngoài màn hình bên trái
    const speed = 0.8;

    function animateText() {
        position += speed; // Tăng vị trí để chạy từ trái sang phải
        const resetPosition = window.innerWidth; // Điểm reset là chiều rộng màn hình

        // Khi chữ đi hết màn hình bên phải, đặt lại vị trí ở ngoài màn hình bên trái
        if (position > resetPosition) {
            position = -bgText.clientWidth;
        }

        bgText.style.transform = `translateX(${position}px) translateY(-50%)`;
        requestAnimationFrame(animateText);
    }

    animateText();
});

document.addEventListener("DOMContentLoaded", function () {
    const roomPackageSelect = document.getElementById("RoomPackage");
    const numberOfParticipantsSelect = document.getElementById("NumberOfParticipants");

    function toggleNumberOfParticipants() {
        if (roomPackageSelect.value === "Private Zone – 69k") {
            numberOfParticipantsSelect.value = "1"; // Set default to 1
            numberOfParticipantsSelect.disabled = true; // Disable selection
        } else {
            numberOfParticipantsSelect.disabled = false; // Enable selection
        }
    }

    // Run on page load
    toggleNumberOfParticipants();

    // Run when RoomPackage changes
    roomPackageSelect.addEventListener("change", toggleNumberOfParticipants);
});

