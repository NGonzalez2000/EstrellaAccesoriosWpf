using CommunityToolkit.Mvvm.ComponentModel;

namespace EstrellaAccesoriosWpf.Models;

public partial class Category : ObservableObject
{
    public Guid Id { get; set; }
    [ObservableProperty]
    private string description;
    [ObservableProperty]
    private List<SubCategory> subCategories;
    private Category(Guid id, string description)
    {
        Id = id;
        Description = description;
        SubCategories = [SubCategory.Create("SIN SUBCATEGORIA")];
    }
    public static Category Create(string description)
    {
        return new Category(Guid.NewGuid(), description);
    }
    public void AddSubCategory(SubCategory subCategory)
    {
        SubCategories.Add(subCategory);
    }
    public void UpdateSubCategory(SubCategory subCategory)
    {
        SubCategory temp = SubCategories.First(sc => sc.Id == subCategory.Id);
        temp.Description = subCategory.Description;
    }
    public void DeleteSubCategory(SubCategory subCategory)
    {
        SubCategories.Remove(subCategory);
    }
    public void Update(Category category)
    {
        Description = category.Description;
    }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public Category() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
