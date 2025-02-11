using UnityEngine;

public class PlayerMovementContext {
    private Player player;
    private PlayerVisual playerVisual;

    // Constructor
    public PlayerMovementContext(Player player, PlayerVisual playerVisual) {
        this.player = player;
        this.playerVisual = playerVisual;
    }

    // Read only properties
    public Player Player => player;
    public PlayerVisual PlayerVisual => playerVisual;
}
