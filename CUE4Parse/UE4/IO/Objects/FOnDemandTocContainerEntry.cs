using System;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using FIoBlockHash = uint;

namespace CUE4Parse.UE4.IO.Objects
{
    public class FOnDemandTocContainerEntry : IOnDemandContainerEntry
    {
        public FIoContainerId ContainerId;
        public string Name { get; }
        public readonly string EncryptionKeyGuid;
        public IOnDemandTocEntry[] Entries { get; }
        public readonly uint[] BlockSizes;
        public readonly FIoBlockHash[] BlockHashes; // FIoBlockHash is just uint32
        public readonly byte[] Header;
        public FSHAHash Hash { get; }
        public readonly EOnDemandContainerFlags ContainerFlags;

        public FOnDemandTocContainerEntry(FArchive Ar, EOnDemandTocVersion version)
        {
            if (version >= EOnDemandTocVersion.ContainerId)
            {
                ContainerId = Ar.Read<FIoContainerId>();
            }

            Name = Ar.ReadFString();
            EncryptionKeyGuid = Ar.ReadFString();
            Entries = Ar.ReadArray(() => new FOnDemandTocEntry(Ar));
            BlockSizes = Ar.ReadArray<uint>();
            BlockHashes = Ar.ReadArray<FIoBlockHash>();
            Hash = new FSHAHash(Ar);

            if (version >= EOnDemandTocVersion.ContainerFlags)
            {
                ContainerFlags = Ar.Read<EOnDemandContainerFlags>();
            }

            if (version >= EOnDemandTocVersion.ContainerHeader)
            {
                Header = Ar.ReadArray<byte>();
            }
        }
    }

    [Flags]
    public enum EOnDemandContainerFlags : byte
    {
        None					= 0,
        PendingEncryptionKey	= (1 << 0),
        Mounted					= (1 << 1),
        StreamOnDemand			= (1 << 2),
        InstallOnDemand			= (1 << 3),
        Encrypted				= (1 << 4),
        WithSoftReferences		= (1 << 5),
        PendingHostGroup		= (1 << 6),
        Last = PendingHostGroup
    }
}
