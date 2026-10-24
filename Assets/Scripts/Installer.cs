using DefaultNamespace;
using Reflex.Core;
using Reflex.Enums;
using UnityEngine;
using Resolution = Reflex.Enums.Resolution;

public class Installer : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerBuilder builder)
    {
        builder.RegisterType(
            typeof(GameRepository),
            new[] { typeof(IGameRepository) },
            Lifetime.Singleton,
            Resolution.Lazy);

        builder.RegisterType(
            typeof(GameService),
            new[] { typeof(IGameService) },
            Lifetime.Singleton,
            Resolution.Lazy);
    }
}