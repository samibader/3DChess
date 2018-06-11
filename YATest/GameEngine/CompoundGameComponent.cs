using System.Collections.Generic;
using Microsoft.Xna.Framework;
using YATest.Utilities;

namespace YATest.GameEngine
{
    class CompoundGameComponent : DrawableGameComponent
    {
        protected List<GameComponent> subComponents;
        protected CompoundGameComponent parent;

        public List<GameComponent> SubComponents
        {
            get { return subComponents; }
        }

        public CompoundGameComponent(Game game, CompoundGameComponent parent) : base(game) { //tree-structure
            subComponents = new List<GameComponent>();
            this.parent = parent;
            if (parent != null)
                parent.SubComponents.Add(this); //attach this node to the tree
        }

        public override void Update(GameTime gameTime)
        {
            //check each subcomponent and Update it if it needs to
            foreach (GameComponent gdc in subComponents)
                if (gdc.Enabled == true)
                    gdc.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //check each subcomponent and Draw it if it needs to
            foreach (GameComponent gdc in subComponents)
                if (gdc is DrawableGameComponent)
                    if (((DrawableGameComponent)gdc).Visible == true)
                        ((DrawableGameComponent)gdc).Draw(gameTime);

            base.Draw(gameTime);
        }

        public void blockControls()
        {
            foreach (GameComponent gdc in subComponents)
                if (gdc is IControllable)
                    ((IControllable)gdc).Blocked = true;
        }

        public void unblockControls()
        {
            foreach (GameComponent gdc in subComponents)
                if (gdc is IControllable)
                    ((IControllable)gdc).Blocked = false;
        }
    }
}
