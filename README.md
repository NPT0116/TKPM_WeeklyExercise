# Quản Lý Sinh Viên - Version 2.0

## Giới Thiệu

Ứng dụng Quản Lý Sinh Viên cho phép người dùng thực hiện các thao tác CRUD (Tạo, Đọc, Cập nhật, Xóa) trên thông tin sinh viên. Version 2.0 được phát triển dựa trên phiên bản trước với các tính năng bổ sung nhằm tăng cường hiệu quả quản lý và hỗ trợ người dùng:

- **Tích hợp import/export dữ liệu**: Hỗ trợ xuất dữ liệu dưới dạng JSON và Excel, cũng như nhập dữ liệu từ file Excel.
- **Tính năng tìm kiếm nâng cao**: Cho phép tìm kiếm sinh viên theo khoa và theo tên (kết hợp với khoa).
- **Hỗ trợ đổi tên và thêm mới**: Cho phép thay đổi tên và thêm mới các đối tượng liên quan như Khoa, Tình trạng sinh viên, và Chương trình đào tạo.
- **Logging & Audit**: Tích hợp logging để hỗ trợ troubleshooting và audit.
- **Hiển thị phiên bản và ngày build**: Cho phép người dùng xem thông tin phiên bản ứng dụng và ngày build.

## Hướng Dẫn Sử Dụng Version 2.0

### 1. Tính Năng Import/Export Dữ Liệu

- **Export Dữ Liệu**:  
  Người dùng có thể xuất dữ liệu sinh viên ra file JSON hoặc Excel thông qua các nút “Xuất dữ liệu JSON” và “Xuất dữ liệu Excel”.
- **Import Dữ Liệu**:  
  Người dùng chọn file Excel (định dạng \*.xlsx) chứa dữ liệu sinh viên theo template đã cung cấp và upload file để nhập dữ liệu vào hệ thống.  
  _**Lưu ý**: File Excel mẫu cần có các cột: StudentId, FullName, DateOfBirth, Gender, FacultyId, Course, ProgramId, Address, Email, PhoneNumber, StatusId._

### 2. Tính Năng Tìm Kiếm Nâng Cao

- **Tìm theo MSSV**: Cho phép tìm kiếm sinh viên theo mã số sinh viên.
- **Tìm theo Khoa**: Cho phép lọc danh sách sinh viên theo khoa.
- **Tìm theo Khoa + Tên Sinh Viên**: Cho phép tìm kiếm theo sự kết hợp giữa khoa và tên sinh viên.

### 3. Tính Năng Quản Lý Đối Tượng Khác

- **Khoa (Faculty)**: Cho phép đổi tên và thêm mới các khoa.
- **Tình Trạng Sinh Viên (Student Status)**: Cho phép đổi tên và thêm mới các tình trạng sinh viên.
- **Chương Trình Đào Tạo (Program)**: Cho phép đổi tên và thêm mới các chương trình đào tạo.

### 4. Logging & Audit

Hệ thống tích hợp logging nhằm:

- Ghi lại các thao tác nhập/xuất dữ liệu.
- Ghi log các lỗi phát sinh trong quá trình xử lý, giúp troubleshooting và audit sau này.

### 5. Hiển Thị Phiên Bản và Ngày Build

Ứng dụng hiện tại cho phép người dùng xem phiên bản và ngày build của ứng dụng. Thông tin này sẽ được hiển thị trên giao diện import/export và được lấy từ backend qua endpoint `/api/AppInfo/version`.

## Demo Video

Bạn có thể xem clip demo các tính năng mới của Version 2.0 tại đây:  
[Demo Video Version 2](https://drive.google.com/file/d/1604EpAfp3aGwsyvBmw78_7ICM_SWNWsJ/view?usp=sharing)

## Hình Ảnh Minh Chứng

Trong thư mục `screenshots/` của repository, bạn sẽ tìm thấy các hình ảnh minh chứng cho các tính năng mới như:

- Giao diện xuất dữ liệu JSON và Excel.
- Giao diện import file Excel.
- Giao diện tìm kiếm nâng cao theo khoa và tên sinh viên.
- Hiển thị thông tin phiên bản và ngày build.

## Các Yêu Cầu Chưa Hoàn Thành / Chưa Làm Kịp

- **Một số tính năng giao diện nâng cao**: Chưa có sự tinh chỉnh giao diện cho các màn hình quản lý đối tượng (Khoa, Tình trạng, Chương trình) theo tiêu chuẩn responsive.
- **Cải thiện chức năng tìm kiếm**: Một số yêu cầu về tính năng tìm kiếm nâng cao vẫn cần được tối ưu.
- **Báo cáo lỗi và thống kê audit**: Chức năng ghi log đã được tích hợp, tuy nhiên cần bổ sung báo cáo thống kê lỗi trong môi trường production.

## Cài Đặt & Chạy Ứng Dụng

1. **Clone repository về máy:**
   ```bash
   git clone <repository-url>
   ```
