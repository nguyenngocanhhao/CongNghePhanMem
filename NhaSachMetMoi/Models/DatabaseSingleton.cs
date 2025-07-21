using NhaSachMetMoi.Models;

public class DatabaseSingleton
{
    private static DatabaseSingleton _instance;
    private static readonly object _lock = new object();
    private NhaSachEntities _context;

    // Constructor private để ngăn chặn khởi tạo trực tiếp từ bên ngoài
    private DatabaseSingleton()
    {
        _context = new NhaSachEntities();
    }

    // Phương thức để lấy instance duy nhất
    public static DatabaseSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseSingleton();
                    }
                }
            }
            return _instance;
        }
    }

    // Phương thức để lấy DbContext
    public NhaSachEntities DbContext => _context;
}
