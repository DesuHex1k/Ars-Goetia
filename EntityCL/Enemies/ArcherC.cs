using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace EntityCL.Enemies
{
    public class ArcherC : EnemyAC
    {
        public Arrow? arrow { get; private set; }
        private readonly ArcherCombat _combat;
        private readonly IArrowFactory _arrowFactory;

        public ArcherC(Player mainPlayer, IArrowFactory arrowFactory) : base(mainPlayer)
        {
            _arrowFactory = arrowFactory;
            _combat = new ArcherCombat(this, _arrowFactory);

            InitializeEntity();
        }

        private void InitializeEntity()
        {
            MAXHealthPoints = 5;
            HealthPoints = MAXHealthPoints;
            AttackDamage = 1;
            EntityName = "Old Archer";
            ImuneState = false;
            IsDead = false;
            SoulCoins = 15;
            Strength = 15;

            EntityRect.Tag = "archerTag";
            EntityRect.Height = 50;
            EntityRect.Width = 50;
            EntityRect.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Archer/ArcherEnemy.png"))
            };
        }

        public override void SetEntityBehavior(List<Rectangle> itemRemover)
        {
            SetHitbox();
            LookToPlayer(MainPlayer.EntityRect);
            Moving();
            _combat.HandleArrowMovement(itemRemover);

            Death(itemRemover);
            TakeDamageFrom();
        }

        public void ShootArrow(Canvas gameScreen)
        {
            _combat.TryShootArrow(gameScreen, MainPlayer);
        }

        public override void Moving()
        {
            if (!CanMove || !IsPlayerNearby()) return;

            base.Moving();
            MoveTowardsPlayer();
        }

        private bool IsPlayerNearby()
        {
            double distanceX = Math.Abs(Canvas.GetLeft(EntityRect) - Canvas.GetLeft(MainPlayer.EntityRect));
            double distanceY = Math.Abs(Canvas.GetTop(EntityRect) - Canvas.GetTop(MainPlayer.EntityRect));
            return distanceX < 100 && distanceY < 100;
        }

        private void MoveTowardsPlayer()
        {
            if (RotateWay.ScaleX == 1)
                Canvas.SetLeft(EntityRect, Canvas.GetLeft(EntityRect) - 1);
            else if (RotateWay.ScaleX == -1)
                Canvas.SetLeft(EntityRect, Canvas.GetLeft(EntityRect) + 1);

            if (Canvas.GetTop(EntityRect) > Canvas.GetTop(MainPlayer.EntityRect) + MainPlayer.EntityRect.Height)
                Canvas.SetTop(EntityRect, Canvas.GetTop(EntityRect) + 1);
            else if (Canvas.GetTop(EntityRect) < Canvas.GetTop(MainPlayer.EntityRect) - MainPlayer.EntityRect.Height)
                Canvas.SetTop(EntityRect, Canvas.GetTop(EntityRect) - 1);
        }

        public override void WallHit()
        {
            CanMove = false;
        }
    }
}

