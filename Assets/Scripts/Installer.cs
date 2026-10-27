using Reflex.Core;
using Reflex.Enums;
using UnityEngine;
using Resolution = Reflex.Enums.Resolution;

public class Installer : MonoBehaviour, IInstaller
{
    [SerializeField] private GameConfig gameConfig;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.RegisterValue(gameConfig);

        builder.RegisterType(
            typeof(PlayerRepository),
            new[] { typeof(IPlayerRepository) },
            Lifetime.Singleton,
            Resolution.Lazy);


        builder.RegisterType(
            typeof(PlayerService),
            new[] { typeof(IPlayerService) },
            Lifetime.Singleton,
            Resolution.Lazy);


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