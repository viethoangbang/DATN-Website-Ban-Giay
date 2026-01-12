# 🔧 HƯỚNG DẪN CẤU HÌNH IPN URL CHO VNPAY

## ⚠️ **LƯU Ý QUAN TRỌNG:**

**IPN URL trống KHÔNG phải nguyên nhân chính của lỗi "Invalid signature" (code 70).**

Lỗi "Invalid signature" thường do:
- ❌ HashSecret không đúng
- ❌ Cách tính hash không đúng (thiếu replace %20 → +)
- ❌ Thứ tự parameters không đúng

**IPN URL chỉ cần cho server-to-server callback, không ảnh hưởng đến việc tạo payment URL.**

---

## 📋 **CẤU HÌNH IPN URL (TÙY CHỌN):**

### **Cho Sandbox (Development):**

1. **Đăng nhập VNPay Merchant Dashboard:**
   - URL: https://sandbox.vnpayment.vn/merchantv2/
   - Email: `jacknolit.id@gmail.com`
   - Password: (mật khẩu bạn đã đặt khi đăng ký)

2. **Truy cập Terminal Settings:**
   - Vào: https://sandbox.vnpayment.vn/merchantv2/Account/TerminalEdit.htm
   - Hoặc: Menu → Thông tin tài khoản → Chọn Terminal `3DTC9ZNX` → Sửa

3. **Cập nhật IPN URL:**
   - **IPN URL**: `http://localhost:5166/api/Checkout/payment-ipn`
   - ⚠️ **Lưu ý**: Localhost không thể truy cập từ VNPay server
   - ✅ **Giải pháp**: Dùng ngrok hoặc để trống (sandbox không bắt buộc)

### **Cho Production:**

- IPN URL phải là **public URL** (không phải localhost)
- VD: `https://yourdomain.com/api/Checkout/payment-ipn`
- Phải có HTTPS
- Phải accessible từ internet

---

## 🚀 **SỬ DỤNG NGROK (CHO DEVELOPMENT):**

Nếu muốn test IPN callback trong development:

1. **Cài đặt ngrok:**
   ```bash
   npm install -g ngrok
   # hoặc
   brew install ngrok
   ```

2. **Chạy ngrok:**
   ```bash
   ngrok http 5166
   ```

3. **Copy public URL:**
   - VD: `https://abc123.ngrok.io`
   - IPN URL: `https://abc123.ngrok.io/api/Checkout/payment-ipn`

4. **Cập nhật trong VNPay dashboard:**
   - Paste URL vào IPN URL field
   - Lưu lại

---

## ✅ **KẾT LUẬN:**

- **IPN URL trống KHÔNG gây lỗi "Invalid signature"**
- **Vấn đề chính**: Cách tính hash (đã fix: replace %20 → +)
- **IPN URL**: Chỉ cần cho production, sandbox có thể bỏ qua

**Focus vào fix signature calculation trước!**


