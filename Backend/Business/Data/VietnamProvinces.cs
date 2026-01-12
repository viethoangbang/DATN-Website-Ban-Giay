namespace Business.Data;

public static class VietnamProvinces
{
    public static List<ProvinceInfo> GetAllProvinces()
    {
        return new List<ProvinceInfo>
        {
            new ProvinceInfo { Code = "01", Name = "Hà Nội" },
            new ProvinceInfo { Code = "02", Name = "Hà Giang" },
            new ProvinceInfo { Code = "04", Name = "Cao Bằng" },
            new ProvinceInfo { Code = "06", Name = "Bắc Kạn" },
            new ProvinceInfo { Code = "08", Name = "Tuyên Quang" },
            new ProvinceInfo { Code = "10", Name = "Lào Cai" },
            new ProvinceInfo { Code = "11", Name = "Điện Biên" },
            new ProvinceInfo { Code = "12", Name = "Lai Châu" },
            new ProvinceInfo { Code = "14", Name = "Sơn La" },
            new ProvinceInfo { Code = "15", Name = "Yên Bái" },
            new ProvinceInfo { Code = "17", Name = "Hoà Bình" },
            new ProvinceInfo { Code = "19", Name = "Thái Nguyên" },
            new ProvinceInfo { Code = "20", Name = "Lạng Sơn" },
            new ProvinceInfo { Code = "22", Name = "Quảng Ninh" },
            new ProvinceInfo { Code = "24", Name = "Bắc Giang" },
            new ProvinceInfo { Code = "25", Name = "Phú Thọ" },
            new ProvinceInfo { Code = "26", Name = "Vĩnh Phúc" },
            new ProvinceInfo { Code = "27", Name = "Bắc Ninh" },
            new ProvinceInfo { Code = "30", Name = "Hải Dương" },
            new ProvinceInfo { Code = "31", Name = "Hải Phòng" },
            new ProvinceInfo { Code = "33", Name = "Hưng Yên" },
            new ProvinceInfo { Code = "34", Name = "Thái Bình" },
            new ProvinceInfo { Code = "35", Name = "Hà Nam" },
            new ProvinceInfo { Code = "36", Name = "Nam Định" },
            new ProvinceInfo { Code = "37", Name = "Ninh Bình" },
            new ProvinceInfo { Code = "38", Name = "Thanh Hóa" },
            new ProvinceInfo { Code = "40", Name = "Nghệ An" },
            new ProvinceInfo { Code = "42", Name = "Hà Tĩnh" },
            new ProvinceInfo { Code = "44", Name = "Quảng Bình" },
            new ProvinceInfo { Code = "45", Name = "Quảng Trị" },
            new ProvinceInfo { Code = "46", Name = "Thừa Thiên Huế" },
            new ProvinceInfo { Code = "48", Name = "Đà Nẵng" },
            new ProvinceInfo { Code = "49", Name = "Quảng Nam" },
            new ProvinceInfo { Code = "51", Name = "Quảng Ngãi" },
            new ProvinceInfo { Code = "52", Name = "Bình Định" },
            new ProvinceInfo { Code = "54", Name = "Phú Yên" },
            new ProvinceInfo { Code = "56", Name = "Khánh Hòa" },
            new ProvinceInfo { Code = "58", Name = "Ninh Thuận" },
            new ProvinceInfo { Code = "60", Name = "Bình Thuận" },
            new ProvinceInfo { Code = "62", Name = "Kon Tum" },
            new ProvinceInfo { Code = "64", Name = "Gia Lai" },
            new ProvinceInfo { Code = "66", Name = "Đắk Lắk" },
            new ProvinceInfo { Code = "67", Name = "Đắk Nông" },
            new ProvinceInfo { Code = "68", Name = "Lâm Đồng" },
            new ProvinceInfo { Code = "70", Name = "Bình Phước" },
            new ProvinceInfo { Code = "72", Name = "Tây Ninh" },
            new ProvinceInfo { Code = "74", Name = "Bình Dương" },
            new ProvinceInfo { Code = "75", Name = "Đồng Nai" },
            new ProvinceInfo { Code = "77", Name = "Bà Rịa - Vũng Tàu" },
            new ProvinceInfo { Code = "79", Name = "Hồ Chí Minh" },
            new ProvinceInfo { Code = "80", Name = "Long An" },
            new ProvinceInfo { Code = "82", Name = "Tiền Giang" },
            new ProvinceInfo { Code = "83", Name = "Bến Tre" },
            new ProvinceInfo { Code = "84", Name = "Trà Vinh" },
            new ProvinceInfo { Code = "86", Name = "Vĩnh Long" },
            new ProvinceInfo { Code = "87", Name = "Đồng Tháp" },
            new ProvinceInfo { Code = "89", Name = "An Giang" },
            new ProvinceInfo { Code = "91", Name = "Kiên Giang" },
            new ProvinceInfo { Code = "92", Name = "Cần Thơ" },
            new ProvinceInfo { Code = "93", Name = "Hậu Giang" },
            new ProvinceInfo { Code = "94", Name = "Sóc Trăng" },
            new ProvinceInfo { Code = "95", Name = "Bạc Liêu" },
            new ProvinceInfo { Code = "96", Name = "Cà Mau" }
        };
    }
}

public class ProvinceInfo
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

