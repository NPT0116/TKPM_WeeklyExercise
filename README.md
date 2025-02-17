# Quản Lý Sinh Viên

## Giới thiệu

Ứng dụng Quản Lý Sinh Viên cho phép người dùng thực hiện các chức năng CRUD (Tạo, Đọc, Cập nhật, Xóa) trên thông tin sinh viên. Ứng dụng được xây dựng với:

- **Backend**: .NET
- **Frontend**: React
- **Cơ sở dữ liệu**: PostgreSQL

## Chức năng

### 1. Thêm Sinh Viên

Người dùng có thể nhập thông tin sinh viên mới, bao gồm:

- Mã số sinh viên (MSSV)
- Họ và tên
- Ngày sinh
- Giới tính
- Khoa
- Địa chỉ
- Email
- Số điện thoại

**Kiểm tra lỗi**: Ứng dụng sẽ kiểm tra tính hợp lệ của các trường thông tin, đảm bảo rằng không có trường nào bị bỏ trống và các định dạng (như email) là chính xác.

### 2. Xem Danh Sách Sinh Viên

Người dùng có thể xem danh sách tất cả sinh viên đã được thêm vào hệ thống. Danh sách sẽ hiển thị các thông tin cơ bản như MSSV, họ tên, khoa và tình trạng học tập.

### 3. Cập Nhật Thông Tin Sinh Viên

Người dùng có thể chọn một sinh viên trong danh sách và cập nhật thông tin của họ. Tất cả các trường thông tin đều có thể được chỉnh sửa.

**Kiểm tra lỗi**: Tương tự như chức năng thêm, ứng dụng sẽ kiểm tra tính hợp lệ của các trường thông tin trước khi lưu thay đổi.

### 4. Xóa Sinh Viên

Người dùng có thể xóa thông tin của một sinh viên khỏi hệ thống. Sau khi xác nhận, sinh viên sẽ được xóa khỏi danh sách.

**Kiểm tra lỗi**: Ứng dụng sẽ thông báo nếu có lỗi xảy ra trong quá trình xóa, chẳng hạn như không tìm thấy sinh viên.

## Công Nghệ Sử Dụng

- **.NET**: Được sử dụng để xây dựng API backend.
- **React**: Được sử dụng để xây dựng giao diện người dùng.
- **PostgreSQL**: Được sử dụng để lưu trữ dữ liệu sinh viên.

## Cài Đặt

1. Clone repository về máy:
   ```bash
   git clone <repository-url>
   ```
2. Cài đặt các gói cần thiết cho backend và frontend:
   ```bash
   cd backend
   dotnet restore
   cd ../frontend
   npm install
   ```
3. Cấu hình kết nối đến cơ sở dữ liệu PostgreSQL trong file cấu hình của backend.
4. Chạy ứng dụng:
   - Backend:
     ```bash
     dotnet run
     ```
   - Frontend:
     ```bash
     npm start
     ```

## Kết Luận

Ứng dụng Quản Lý Sinh Viên cung cấp một giải pháp đơn giản và hiệu quả để quản lý thông tin sinh viên với các chức năng CRUD đầy đủ và kiểm tra lỗi linh động. Hãy thử nghiệm và đóng góp ý kiến để cải thiện ứng dụng!
