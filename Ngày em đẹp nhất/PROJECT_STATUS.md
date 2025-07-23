# TechnicalSupportApi - Project Status Report

## 1. Tổng quan dự án
- **Mục tiêu:** Hệ thống quản lý ticket hỗ trợ kỹ thuật realtime (ASP.NET Core 9, SignalR, React.js, SQL Server)
- **Backend:** ASP.NET Core 9 Web API, SignalR, Identity, JWT, EF Core
- **Frontend:** Sẽ phát triển sau (React.js)

---

## 2. Tiến độ so với TODO List

### Phase 0: Thiết lập Dự án & Thiết kế Cơ sở dữ liệu
- [x] Khởi tạo project ASP.NET Core Web API
- [x] Thiết kế các model chính: Ticket, ApplicationUser, Comment, Status, Group, Attachment, TechnicianGroup
- [x] Thiết lập ApplicationDbContext, cấu hình quan hệ, code first
- [x] Thiết kế các API endpoint (Auth, Ticket, ...)
- [x] Tạo migration và cập nhật database (Code First)

### Phase 1: Xây dựng Backend & API Core
- [x] Cấu hình EF Core, ASP.NET Core Identity, JWT
- [x] Tạo AuthController (đăng ký, đăng nhập, trả về JWT)
- [x] Tạo TicketsController (CRUD ticket, cập nhật trạng thái, thêm comment)
- [x] Bảo vệ endpoint bằng [Authorize], phân quyền cơ bản
- [ ] Seed dữ liệu mẫu (Status, Group, ... nếu cần)

### Phase 3: Tích hợp Realtime (SignalR)
- [x] Tạo SignalR Hub (TicketHub.cs)
- [x] Inject IHubContext vào controller, gọi realtime khi cập nhật trạng thái/comment
- [x] Đã map SignalR Hub trong Program.cs

### Phase 4: Tính năng bổ sung
- [ ] Gửi email thông báo (MailKit, IEmailSender)
- [ ] Chức năng tìm kiếm & lọc ticket (query param cho GET /api/tickets)

### Phase 5: Kiểm thử & Demo
- [x] Có thể kiểm thử API qua Swagger UI (http://localhost:5073/swagger)
- [ ] Chưa có tài liệu chi tiết cho frontend (có thể bổ sung sau)

---

## 3. Chi tiết các thành phần backend

### 3.1. Các model chính
- **ApplicationUser**: kế thừa IdentityUser, thêm DisplayName, Expertise
- **Ticket**: TicketId, Title, Description, CustomerId, AssigneeId, GroupId, StatusId, Priority, CreatedAt, UpdatedAt, ClosedAt
- **Comment**: CommentId, TicketId, UserId, Content, CreatedAt
- **Status**: StatusId, Name
- **Group**: GroupId, Name, Description
- **Attachment**: AttachmentId, TicketId, CommentId, UploadedById, OriginalFileName, StoredPath
- **TechnicianGroup**: UserId, GroupId (nhiều-nhiều)

### 3.2. Các endpoint API đã có
- **AuthController**
  - `POST /api/auth/register` – Đăng ký user mới
  - `POST /api/auth/login` – Đăng nhập, trả về JWT
- **TicketsController**
  - `GET /api/tickets` – Lấy danh sách ticket (theo vai trò)
  - `GET /api/tickets/{id}` – Lấy chi tiết ticket
  - `POST /api/tickets` – Tạo ticket mới
  - `PUT /api/tickets/{id}/status` – Cập nhật trạng thái ticket
  - `POST /api/tickets/{id}/comments` – Thêm comment vào ticket

### 3.3. SignalR Hub
- **TicketHub.cs**: Đã có, backend đã sẵn sàng cho realtime (frontend chỉ cần kết nối tới `/ticketHub`)

---

## 4. Hướng dẫn seed dữ liệu mẫu (Status, Group, ...)
- Có thể seed dữ liệu trong `Program.cs` hoặc tạo migration seed.
- Ví dụ seed Status:
```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!db.Statuses.Any())
    {
        db.Statuses.AddRange(
            new Status { Name = "Open" },
            new Status { Name = "In Progress" },
            new Status { Name = "Closed" }
        );
        db.SaveChanges();
    }
}
```
- Tương tự cho Group, Role, ...

---

## 5. Hướng dẫn kiểm thử API

### 5.1. Kiểm thử với Swagger UI
- Chạy backend: `dotnet run`
- Truy cập: [http://localhost:5073/swagger](http://localhost:5073/swagger)
- Thực hiện các thao tác: đăng ký, đăng nhập, tạo ticket, ...

### 5.2. Kiểm thử với Postman
- Đăng ký user:
  - `POST http://localhost:5073/api/auth/register`
  - Body (JSON):
    ```json
    {
      "email": "test@example.com",
      "password": "YourPassword123",
      "displayName": "Test User"
    }
    ```
- Đăng nhập:
  - `POST http://localhost:5073/api/auth/login`
  - Body (JSON):
    ```json
    {
      "email": "test@example.com",
      "password": "YourPassword123"
    }
    ```
  - Lấy token trả về, dùng cho các request tiếp theo:
    - Header: `Authorization: Bearer <token>`
- Tạo ticket:
  - `POST http://localhost:5073/api/tickets`
  - Body (JSON):
    ```json
    {
      "title": "Lỗi máy in",
      "description": "Máy in không nhận lệnh.",
      "statusId": 1,
      "priority": "High"
    }
    ```

---

## 6. Lưu ý kỹ thuật cho backend/frontend
- **Backend:**
  - Đã sẵn sàng cho tích hợp frontend (React, SignalR, JWT)
  - Có thể mở rộng thêm các tính năng nâng cao (email, tìm kiếm, phân quyền chi tiết)
  - Nên seed dữ liệu mẫu (Status, Group, Role) trước khi bàn giao cho frontend
- **Frontend:**
  - Cần đăng nhập lấy JWT token để gọi các API bảo vệ
  - Kết nối SignalR tới `/ticketHub` để nhận realtime
  - Tham khảo Swagger UI để biết cấu trúc request/response

---

**Cập nhật: [ngày tạo file này]** 