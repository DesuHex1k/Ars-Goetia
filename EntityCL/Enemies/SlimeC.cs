using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace EntityCL.Enemies
{
    public class SlimeC : EnemyAC
    {
        private readonly Random random = new();
        private int Speed { get; set; }

        private const int GAME_FIELD_WIDTH = 1540;
        private const int GAME_FIELD_PADDING = 5;
        private const int WALL_HIT_OFFSET = 5;

        public SlimeC(Player mainPlayer) 
            : this(mainPlayer, 35, 30, 1) { }

        public SlimeC(Player mainPlayer, int width, int height, int speed) 
            : base(mainPlayer)
        {
            Speed = speed;
            InitializeSlime(width, height);
        }

        private void InitializeSlime(int width, int height)
        {
            MAXHealthPoints = 1;
            HealthPoints = MAXHealthPoints;
            AttackDamage = 1;
            EntityName = "Acid Slime";
            SoulCoins = 1;
            Strength = 0;

            RotateWay.ScaleX = (random.Next(0, 2) * 2 - 1); // Генерує -1 або 1

            EntityRect.Tag = "slimeTag";
            EntityRect.Height = height;
            EntityRect.Width = width;
            EntityRect.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Slime/GreenSlime.png"))
            };
        }

        public override void SetEntityBehavior(List<Rectangle> itemRemover)
        {
            SetHitbox();
            Moving();
            Attack();
            Death(itemRemover);
            TakeDamageFrom();
        }

        public override void Moving()
        {
            if (IsDead) return;

            double currentX = Canvas.GetLeft(EntityRect);
            double newX = currentX + (Speed * RotateWay.ScaleX);

            if (newX > GAME_FIELD_WIDTH - EntityRect.Width || newX < GAME_FIELD_PADDING)
            {
                WallHit();
                return;
            }

            Canvas.SetLeft(EntityRect, newX);
        }

        public override void WallHit()
        {
            RotateWay.ScaleX *= -1;
            Canvas.SetLeft(EntityRect, Canvas.GetLeft(EntityRect) + (WALL_HIT_OFFSET * RotateWay.ScaleX));
        }

        public override void Attack()
        {
            if (IsDead) return;

            if (EntityHitBox.IntersectsWith(MainPlayer.EntityHitBox))
            {
                HealthPoints--;
                MainPlayer.TakeDamageFrom(this);
            }
        }
    }
}

