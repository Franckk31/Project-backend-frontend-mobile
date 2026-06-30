import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';
import { Users } from '../models/users.model'; // 1. Import โมเดล User เข้ามาเพิ่ม

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = '/api/products';
  private userApiUrl = '/api/users'; // 2. เพิ่ม URL สำหรับดึงข้อมูล User

  constructor(private http: HttpClient) {}

  // ======= ของเดิม (Product) =======
  getAll(): Observable<Product[]> { return this.http.get<Product[]>(this.apiUrl); }
  getById(id: number): Observable<Product> { return this.http.get<Product>(`${this.apiUrl}/${id}`); }
  create(product: Partial<Product>): Observable<Product> { return this.http.post<Product>(this.apiUrl, product); }
  update(id: number, product: Partial<Product>): Observable<void> { return this.http.put<void>(`${this.apiUrl}/${id}`, product); }
  delete(id: number): Observable<void> { return this.http.delete<void>(`${this.apiUrl}/${id}`); }

  // ======= เพิ่มเติม (User) =======
 getUsersAll(): Observable<Users[]> {
    return this.http.get<Users[]>('/api/User'); // 👈 ใช้ User ตัวใหญ่ ไม่มี s ตาม Swagger
  }

  createUser(user: Partial<Users>): Observable<Users> {
    return this.http.post<Users>('/api/User', user);
  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`/api/User/${id}`);
  }
}