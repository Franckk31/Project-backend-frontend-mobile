import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        primarySwatch: Colors.blue,
        useMaterial3: true,
      ),
      home: const ProductListScreen(),
    );
  }
}

// ==================== หน้าหลัก: แสดงรายการสินค้าทั้งหมด ====================
class ProductListScreen extends StatefulWidget {
  const ProductListScreen({super.key});

  @override
  State<ProductListScreen> createState() => _ProductListScreenState();
}

class _ProductListScreenState extends State<ProductListScreen> {
  List _productsList = []; 
  bool _isLoading = false;
  String _errorMessage = "";

  // ฟังก์ชันดึงข้อมูลสินค้า พร้อมระบบป้องกันแอปค้าง (Timeout)
  Future<void> fetchProducts() async {
    setState(() {
      _isLoading = true;
      _errorMessage = ""; 
    });

    final url = Uri.parse('http://10.0.2.2:5000/api/Products'); 

    try {
      // ดักไว้ว่าถ้าเชื่อมต่อภายใน 4 วินาทีไม่ได้ ให้ตัดวงจรทันที ป้องกันหน้าจอค้างค้าง
      final response = await http.get(url).timeout(const Duration(seconds: 4));

      if (response.statusCode == 200) {
        final List data = jsonDecode(response.body);
        setState(() {
          _productsList = data;
          _isLoading = false;
        });
      } else {
        setState(() {
          _errorMessage = "เกิดข้อผิดพลาดจากเซิร์ฟเวอร์: ${response.statusCode}";
          _isLoading = false;
        });
      }
    } catch (e) {
      setState(() {
        _errorMessage = "ไม่สามารถเชื่อมต่อ C# Backend ได้\n(กรุณาตรวจสอบว่าเปิดรัน API หลังบ้านไว้แล้ว)";
        _isLoading = false;
      });
    }
  }

  @override
  void initState() {
    super.initState();
    fetchProducts(); // โหลดข้อมูลเมื่อเปิดแอป
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('รายการสินค้า (Products)'),
        backgroundColor: Colors.blueAccent,
        foregroundColor: Colors.white,
      ),
      body: _isLoading
          ? const Center(child: CircularProgressIndicator()) 
          : _errorMessage.isNotEmpty
              ? Center(
                  child: Padding(
                    padding: const EdgeInsets.all(20.0),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        const Icon(Icons.cloud_off, size: 60, color: Colors.redAccent),
                        const SizedBox(height: 12),
                        Text(
                          _errorMessage, 
                          style: const TextStyle(color: Colors.red, fontSize: 16), 
                          textAlign: TextAlign.center
                        ),
                        const SizedBox(height: 15),
                        ElevatedButton.icon(
                          onPressed: fetchProducts,
                          icon: const Icon(Icons.refresh),
                          label: const Text('ลองใหม่อีกครั้ง'),
                        )
                      ],
                    ),
                  ),
                ) 
              : _productsList.isEmpty
                  ? const Center(child: Text('ไม่มีข้อมูลสินค้าในระบบ')) 
                  : ListView.builder( 
                      itemCount: _productsList.length,
                      itemBuilder: (context, index) {
                        final item = _productsList[index];

                        return Card(
                          margin: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
                          elevation: 2,
                          child: ListTile(
                            leading: CircleAvatar(
                              backgroundColor: Colors.blueAccent,
                              child: Text('${index + 1}', style: const TextStyle(color: Colors.white)),
                            ),
                            title: Text(
                              item['name'] ?? 'ไม่มีชื่อสินค้า',
                              style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 16),
                            ),
                            subtitle: Text('ราคา: ${item['price'] ?? 0} บาท'),
                            trailing: const Icon(Icons.arrow_forward_ios, size: 16, color: Colors.grey),
                            // เมื่อกดจิ้ม จะย้ายหน้าจอไปหน้า Detail ทันที
                            onTap: () {
                              Navigator.push(
                                context,
                                MaterialPageRoute(
                                  builder: (context) => ProductDetailScreen(product: item),
                                ),
                              );
                            },
                          ),
                        );
                      },
                    ),
      floatingActionButton: FloatingActionButton(
        onPressed: fetchProducts, 
        backgroundColor: Colors.blueAccent,
        child: const Icon(Icons.refresh, color: Colors.white),
      ),
    );
  }
}

// ==================== หน้าแสดงรายละเอียดสินค้า (หน้าใหม่มีปุ่มกลับ) ====================
class ProductDetailScreen extends StatelessWidget {
  final Map<String, dynamic> product;

  const ProductDetailScreen({super.key, required this.product});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(product['name'] ?? 'รายละเอียดสินค้า'),
        backgroundColor: Colors.blueAccent,
        foregroundColor: Colors.white,
        // ปุ่มย้อนกลับ (<) บน AppBar จะถูกสร้างขึ้นให้อัตโนมัติโดย Flutter ครับ
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Card(
          elevation: 4,
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
          child: Padding(
            padding: const EdgeInsets.all(20.0),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              mainAxisSize: MainAxisSize.min,
              children: [
                Row(
                  children: const [
                    Icon(Icons.shopping_bag, color: Colors.blueAccent, size: 28),
                    SizedBox(width: 10),
                    Text(
                      'ข้อมูลสินค้าจากระบบ',
                      style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold, color: Colors.blueAccent),
                    ),
                  ],
                ),
                const Divider(height: 30, thickness: 1),
                _buildInfoRow(Icons.fingerprint, 'ID สินค้า:', '${product['id'] ?? '-'}'),
                _buildInfoRow(Icons.label, 'ชื่อสินค้า:', '${product['name'] ?? '-'}'),
                _buildInfoRow(Icons.description, 'รายละเอียด:', '${product['description'] ?? '-'}'),
                _buildInfoRow(Icons.monetization_on, 'ราคา:', '${product['price'] ?? 0} บาท'),
                _buildInfoRow(Icons.layers, 'จำนวนคงเหลือในคลัง:', '${product['stock'] ?? 0} ชิ้น'),
                const SizedBox(height: 25),
                // ปุ่มกดย้อนกลับเพิ่มเติมที่ด้านล่างหน้าจอ
                SizedBox(
                  width: double.infinity,
                  child: ElevatedButton.icon(
                    onPressed: () => Navigator.pop(context),
                    icon: const Icon(Icons.arrow_back),
                    label: const Text('ย้อนกลับหน้าหลัก', style: TextStyle(fontSize: 16)),
                    style: ElevatedButton.styleFrom(
                      backgroundColor: Colors.blueAccent,
                      foregroundColor: Colors.white,
                      padding: const EdgeInsets.symmetric(vertical: 12),
                      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildInfoRow(IconData icon, String label, String value) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(icon, color: Colors.grey[600], size: 20),
          const SizedBox(width: 10),
          Expanded(
            child: RichText(
              text: TextSpan(
                style: const TextStyle(color: Colors.black, fontSize: 16),
                children: [
                  TextSpan(text: '$label ', style: const TextStyle(fontWeight: FontWeight.bold)),
                  TextSpan(text: value, style: TextStyle(color: Colors.grey[800])),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }
}