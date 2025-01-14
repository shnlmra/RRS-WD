namespace RRS.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }

        public ICollection<Menu> Menus { get; set; }
    }
}
