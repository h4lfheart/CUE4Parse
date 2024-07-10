using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.CustomizableObject.Mutable.Meshes;

public class Mesh
{
    public int Version;
    
    public bool IsBroken;
    
    public Mesh(FArchive Ar)
    {
        Version = Ar.Read<int>();
        if (Version == -1)
        {
            IsBroken = true;
            return;
        }

        if (Version > 23)
        {
            throw new ParserException($"Mutable mesh version {Version} is not supported.");
        }
        
        // TODO actually serialize meshes
    }
}