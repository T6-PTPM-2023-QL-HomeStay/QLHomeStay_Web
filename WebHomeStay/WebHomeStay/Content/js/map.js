// Khởi tạo bản đồ
function initMap() {
    // Vị trí ban đầu
    const initialPosition = { lat: 37.7749, lng: -122.4194 };

    // Tạo bản đồ và đặt vị trí ban đầu
    const map = new google.maps.Map(document.getElementById('map'), {
        center: initialPosition,
        zoom: 12 // Mức độ zoom mặc định
    });

    // Đặt đánh dấu tại vị trí ban đầu
    const marker = new google.maps.Marker({
        position: initialPosition,
        map: map,
        title: 'San Francisco, CA'
    });
}
