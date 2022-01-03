namespace SourceGeneratorClassLibrary
{
    public interface IMapper<TSource,TDest>
    {
        TDest Map(TSource src);
    }
}
