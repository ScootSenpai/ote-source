namespace OTE.Data.EFCore.Dtos;

public interface IDto<TEntity>
{
    public TEntity Map();
}
