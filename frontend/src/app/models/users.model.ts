export interface Users {
  userId?: number;      // ตัวพิมพ์เล็กผสมใหญ่ (Camel Case) ตามมาตรฐาน Angular/C#
  username: string;
  password?: string;
  name: string;
  role: string;
  contactInfo: string;  // ใช้ชื่อนี้ให้ตรงกับใน component.ts
}