using Godot;

public interface IEnemy
{

    public void onHit();

    public Area2D hitArea();
    void setPath(Path path);
    void setMaxHealth(int enemyMaxHealth);
    void kill();
}