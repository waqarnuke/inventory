using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Core.Entities.Photo> Photos { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<MobileNetwork> MobileNetworks { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Buying> Buyings { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Company> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            // modelBuilder.Entity<Item>()
            //     .HasMany(i => i.Images)
            //     .WithOne()
            //     .HasForeignKey(i => i.ItemId)
            //     .OnDelete(DeleteBehavior.Cascade); 

            
            modelBuilder.Entity<Company>().HasData(
                new Company{Id=1, Name="Abc"}
            );
            
            modelBuilder.Entity<Brand>().HasData(
                new Brand{Id=1, Name="Apple"},
                new Brand{Id=2, Name="Samsung"},
                new Brand{Id=3, Name="Huawei"},
                new Brand{Id=4, Name="Anonymous"}
            );
            modelBuilder.Entity<Model>().HasData(
                new Model{Id=1, Name="iPhone 11"},
                new Model{Id=2, Name="Galaxy S20"},
                new Model{Id=3, Name="P40 Pro"},
                new Model{Id=4, Name="Anonymous"}
            );
            modelBuilder.Entity<Category>().HasData(
                new Category{Id=1, Name="Action",DisplayOrder=1},
                new Category{Id=2, Name="History",DisplayOrder=2},
                new Category{Id=3, Name="Sifi",DisplayOrder=3}
            );
            modelBuilder.Entity<Product>().HasData(
                new Product { 
                    Id = 1, 
                    Title = "Fortune of Time", 
                    Author="Billy Spark", 
                    Description= "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN="SWD9999001",
                    ListPrice=99,
                    Price=90,
                    Price50=85,
                    Price100=80,
                    CategoryId = 1,
                    ImageUrl=""
                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 1,
                    ImageUrl=""
                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = 2,
                    ImageUrl=""
                },
                new Product
                {
                    Id = 4,
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WS3333333301",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 2,
                    ImageUrl=""
                },
                new Product
                {
                    Id = 5,
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SOTJ1111111101",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 3,
                    ImageUrl=""
                },
                new Product
                {
                    Id = 6,
                    Title = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "FOT000000001",
                    ListPrice = 25,
                    Price = 23,
                    Price50 = 22,
                    Price100 = 20,
                    CategoryId = 3,
                    ImageUrl=""
                } 
            );
            modelBuilder.Entity<Core.Entities.Photo>().HasData(
                new Core.Entities.Photo{Id = 1, FileName = "Action",PictureUrl = "/images/product/placeholder.jpg",IsMain = true},
                new Core.Entities.Photo{Id = 2, FileName = "Action",PictureUrl = "/images/product/placeholder.jpg",IsMain = false},
                new Core.Entities.Photo{Id = 3, FileName = "Action",PictureUrl = "/images/product/placeholder.jpg",IsMain = false},
                new Core.Entities.Photo{Id = 4, FileName = "Action",PictureUrl = "/images/product/placeholder.jpg",IsMain = false},
                new Core.Entities.Photo{Id = 5, FileName = "Action",PictureUrl = "/images/product/placeholder.jpg",IsMain = false},
                new Core.Entities.Photo{Id = 6, FileName = "Action",PictureUrl = "/images/product/placeholder.jpg",IsMain = false}
            );
        
            modelBuilder.Entity<Item>().HasData(
                new Item{Id=1, Title="iPhone 11", Description="I phone 11 test", BrandId=1,ModelId=1,Price=999,Stock=10,ImageUrl="/images/product/placeholder.jpg",LocationId=1,MobileNetworkId=1,StorageId=1,ItemTypeId=1},  
                new Item{Id=2, Title="Galaxy S20", Description="I Glaxy S20 test", BrandId=2,ModelId=2,Price=899,Stock=10,ImageUrl="/images/product/placeholder.jpg",LocationId=2,MobileNetworkId=2,StorageId=2,ItemTypeId=2},
                new Item{Id=3, Title="P40 Pro",  Description="I P40 pro test", BrandId=3,ModelId=3,Price=799,Stock=10,ImageUrl="/images/product/placeholder.jpg",LocationId=3,MobileNetworkId=3,StorageId=3,ItemTypeId=1},
                new Item{Id=4, Title="iPhone 11", Description="I phone 11 test", BrandId=1,ModelId=1,Price=999,Stock=10,ImageUrl="/images/product/placeholder.jpg",LocationId=1,MobileNetworkId=1,StorageId=4,ItemTypeId=1},
                new Item{Id=5, Title="Galaxy S20",Description="I Glaxy S20 test", BrandId=2,ModelId=2,Price=899,Stock=10,ImageUrl="/images/product/placeholder.jpg",LocationId=2,MobileNetworkId=2,StorageId=5,ItemTypeId=1},
                new Item{Id=6, Title="P40 Pro", Description="I P40 pro test",BrandId=3,ModelId=3,Price=799,Stock=10,ImageUrl="/images/product/placeholder.jpg",LocationId=3,MobileNetworkId=3,StorageId=1,ItemTypeId=1},
                new Item{Id=7, Title="Charger", Description="20 chagere", BrandId=4,ModelId=4,Price=10,Stock=10,ImageUrl="/images/product/placeholder.jpg",LocationId=1,MobileNetworkId=8,StorageId=6,ItemTypeId=3},
                new Item{Id=8, Title="Temper Glass", Description="20 Glass", BrandId=4,ModelId=4,Price=10,Stock=10,ImageUrl="/images/product/placeholder.jpg",LocationId=1,MobileNetworkId=8,StorageId=6,ItemTypeId=3}
            );

            modelBuilder.Entity<Location>().HasData(
                new Location{Id=1, Name="Humble", Address="Humble", CompanyId=1},
                new Location{Id=2, Name="Houston", Address="Houston", CompanyId=1},
                new Location{Id=3, Name="Kingwood", Address="Kingwood" , CompanyId=1},
                new Location{Id=4, Name="Atascocita" , Address="Atascocita", CompanyId=1}
            );

            modelBuilder.Entity<MobileNetwork>().HasData(
                new MobileNetwork{Id=1, Name="AT&T"},
                new MobileNetwork{Id=2, Name="Verizon"},
                new MobileNetwork{Id=3, Name="T-Mobile"},
                new MobileNetwork{Id=4, Name="Sprint"},
                new MobileNetwork{Id=5, Name="MetroPCS"},
                new MobileNetwork{Id=6, Name="Cricket"},
                new MobileNetwork{Id=7, Name="Boost Mobile"},
                new MobileNetwork{Id=8, Name="Anonymous"}
            );

            modelBuilder.Entity<Storage>().HasData(
                new Storage{Id=1, Name="32GB"},
                new Storage{Id=2, Name="64GB"},
                new Storage{Id=3, Name="256GB"},
                new Storage{Id=4, Name="500GB"},
                new Storage{Id=5, Name="1TB"},
                new Storage{Id=6, Name="Anonymous"}
            );

            modelBuilder.Entity<ItemType>().HasData(
                new Model{Id=1, Name="Single"},
                new Model{Id=2, Name="Multiple"},
                new Model{Id=3, Name="Anonymous"}
            );
            
            modelBuilder.Entity<Buying>().HasData(
                new Buying{Id=1, ItemId=1, Quantity=0, PricePerUnit=999, TotalPrice=999, PaymentMethod="Cash", UserId=Guid.NewGuid()},
                new Buying{Id=2, ItemId=2, Quantity=0, PricePerUnit=899, TotalPrice=899, PaymentMethod="Card", UserId=Guid.NewGuid()},
                new Buying{Id=3, ItemId=3, Quantity=0, PricePerUnit=799, TotalPrice=799, PaymentMethod="Cash", UserId=Guid.NewGuid()}
            );

            modelBuilder.Entity<Register>().HasData(
                new Register{Id=1, CardBalance=10000,CashBalance=100000}
            );

            modelBuilder.Entity<Sale>().HasData(
                new Sale{Id=1, ItemId=1, Quantity=0, PricePerUnit=999, TotalPrice=999, PaymentMethod="Cash", UserId=Guid.NewGuid(),LocationId=1},
                new Sale{Id=2, ItemId=2, Quantity=0, PricePerUnit=899, TotalPrice=899, PaymentMethod="Card", UserId=Guid.NewGuid(),LocationId=2},
                new Sale{Id=3, ItemId=3, Quantity=0, PricePerUnit=799, TotalPrice=799, PaymentMethod="Cash", UserId=Guid.NewGuid(),LocationId=3}
            );

            modelBuilder.Entity<Image>().HasData(
            new Image { Id = 1, ItemId = 1, Url = "iphone1.jpg" },
            new Image { Id = 2, ItemId = 1, Url = "iphone2.jpg" }

            );

            modelBuilder.Entity<Supplier>().HasData(
                new Supplier{Id=1, CompanyName="Abc", ContactPerson="346-111-2222", Email="abc@gmail.com", Address="331"},
                new Supplier{Id=2, CompanyName="xyz", ContactPerson="346-111-3333", Email="xyz@gmail.com", Address="332"},
                new Supplier{Id=3, CompanyName="aaa", ContactPerson="346-111-4444", Email="aaa@gmail.com", Address="333"}
            );
        }
    }
}