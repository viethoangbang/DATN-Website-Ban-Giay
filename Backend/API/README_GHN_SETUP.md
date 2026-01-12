# 🔧 HƯỚNG DẪN CẤU HÌNH GHN API

## ❌ Lỗi thường gặp: "Lỗi lấy thông tin shop"

Lỗi này xảy ra khi `ShopId` trong `appsettings.json` không đúng hoặc token không có quyền truy cập shop đó.

## ✅ CÁCH KHẮC PHỤC:

### **Bước 1: Lấy danh sách Shop từ GHN**

Sau khi backend đã chạy, gọi API để lấy danh sách shop:

```bash
# Với token đã có
curl -X GET "http://localhost:5166/api/GHN/shops" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

Hoặc dùng Postman/Thunder Client:
- **URL**: `GET http://localhost:5166/api/GHN/shops`
- **Header**: `Authorization: Bearer YOUR_JWT_TOKEN`

### **Bước 2: Kiểm tra ShopId**

Response sẽ trả về danh sách shop:
```json
[
  {
    "shopId": 123456,
    "name": "Shop Sneaker Poly",
    "phone": "0123456789"
  }
]
```

### **Bước 3: Cập nhật appsettings.json**

Copy `shopId` từ response và paste vào `appsettings.json`:

```json
{
  "GHN": {
    "BaseUrl": "https://dev-online-gateway.ghn.vn/shiip/public-api",
    "Token": "YOUR_GHN_TOKEN_HERE",
    "ShopId": "123456",  // ← Dùng shopId từ API response
    "FromDistrictId": "3440"
  }
}
```

### **Bước 4: Restart Backend**

Sau khi cập nhật, restart backend để áp dụng thay đổi.

---

## 📋 **KIỂM TRA TOKEN VÀ SHOPID:**

### **1. Kiểm tra Token có hợp lệ không:**

Gọi API lấy provinces (không cần ShopId):
```bash
GET http://localhost:5166/api/GHNLocation/provinces
```

Nếu trả về danh sách provinces → Token hợp lệ ✅

### **2. Kiểm tra ShopId có đúng không:**

Gọi API lấy shops:
```bash
GET http://localhost:5166/api/GHN/shops
```

Nếu trả về danh sách shop → Kiểm tra `shopId` trong response có khớp với `appsettings.json` không.

---

## ⚠️ **LƯU Ý:**

1. **Sandbox vs Production:**
   - Sandbox: `https://dev-online-gateway.ghn.vn/shiip/public-api`
   - Production: `https://online-gateway.ghn.vn/shiip/public-api`

2. **Token phải có quyền:**
   - Token phải được cấp quyền truy cập shop
   - Nếu không có quyền, liên hệ GHN support

3. **FromDistrictId:**
   - Phải là District ID từ GHN (không phải tên)
   - VD: Hà Nội - Nam Từ Liêm = `3440`
   - Có thể lấy từ API `/api/GHNLocation/districts/{provinceId}`

---

## 🐛 **DEBUG:**

Nếu vẫn lỗi, check logs:
- Backend logs sẽ hiển thị: `GHN error message: ...`
- Kiểm tra `ShopId` trong log có khớp với `appsettings.json` không

