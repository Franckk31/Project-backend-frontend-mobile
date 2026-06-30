# ตัวอย่างโปรเจกต์ C# Web API + Angular

โปรเจกต์ตัวอย่างระบบจัดการสินค้า (Product CRUD) แบ่งเป็น 2 ส่วน:

```
sample-project/
├── backend/                  # C# .NET 8 Web API
│   └── ProductApi/
│       ├── Controllers/
│       │   └── ProductsController.cs   # API endpoints (GET, POST, PUT, DELETE)
│       ├── Models/
│       │   └── Product.cs              # โมเดลข้อมูลสินค้า
│       ├── Services/
│       │   └── ProductService.cs       # Business logic (เก็บข้อมูลใน memory)
│       ├── Program.cs                  # จุดเริ่มต้น + ตั้งค่า CORS
│       ├── appsettings.json
│       └── ProductApi.csproj
│
└── frontend/                 # Angular 18 (standalone components)
    └── src/
        ├── app/
        │   ├── models/product.model.ts
        │   ├── services/product.service.ts     # เรียก API ด้วย HttpClient
        │   ├── product-list/                    # หน้าแสดง/เพิ่ม/ลบสินค้า
        │   └── app.component.ts
        ├── main.ts
        └── package.json
```

## วิธีรัน Backend (C# Web API)

ต้องติดตั้ง [.NET 8 SDK](https://dotnet.microsoft.com/download) ก่อน

```bash
cd backend/ProductApi
dotnet restore
dotnet run
```

API จะรันที่ `https://localhost:5001` (หรือ port ที่ระบบสุ่มให้ ดูได้จาก console เมื่อรัน)
เปิด `https://localhost:5001/swagger` เพื่อดูและทดสอบ API ผ่าน Swagger UI

Endpoints ที่มี:
- `GET    /api/products`     - ดึงรายการสินค้าทั้งหมด
- `GET    /api/products/{id}` - ดึงสินค้าตาม id
- `POST   /api/products`     - เพิ่มสินค้าใหม่
- `PUT    /api/products/{id}` - แก้ไขสินค้า
- `DELETE /api/products/{id}` - ลบสินค้า

## วิธีรัน Frontend (Angular)

ต้องติดตั้ง [Node.js](https://nodejs.org/) และ Angular CLI ก่อน

```bash
cd frontend
npm install
npm start
```

เปิดเบราว์เซอร์ที่ `http://localhost:4200`

> **หมายเหตุ**: ถ้า backend รันที่ port อื่น ให้แก้ไข `apiUrl` ในไฟล์
> `frontend/src/app/services/product.service.ts` ให้ตรงกับ port จริง

## หลักการทำงาน

1. **Backend (C#)**: เปิด REST API ผ่าน ASP.NET Core, มีการตั้งค่า **CORS**
   ใน `Program.cs` เพื่ออนุญาตให้ `http://localhost:4200` (Angular dev server)
   เรียกใช้งานข้าม origin ได้
2. **Frontend (Angular)**: ใช้ `HttpClient` ใน `ProductService` เพื่อยิง
   HTTP request (GET/POST/PUT/DELETE) ไปยัง API ของ C# แล้วแสดงผลผ่าน
   `ProductListComponent`

โปรเจกต์นี้เป็นตัวอย่างพื้นฐาน เหมาะสำหรับนำไปต่อยอด เช่น เปลี่ยนจากเก็บข้อมูล
ใน memory เป็นเชื่อมต่อฐานข้อมูลจริงด้วย Entity Framework Core, เพิ่ม
Authentication/Authorization, หรือเพิ่ม validation เพิ่มเติม
