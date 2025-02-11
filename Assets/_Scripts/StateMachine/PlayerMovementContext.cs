using Unity.Cinemachine;
using UnityEngine;

public class PlayerMovementContext {
    private Player player;
    private PlayerVisual playerVisual;
    private CinemachineCamera camera;

    // Constructor
    public PlayerMovementContext(Player player, PlayerVisual playerVisual, CinemachineCamera camera) {
        this.player = player;
        this.playerVisual = playerVisual;
    }

    // Read only properties
    public Player Player => player;
    public PlayerVisual PlayerVisual => playerVisual;

    public CinemachineCamera Camera => camera;
}
