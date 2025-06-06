using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACE.Server.Physics.Common;
using ACE.Server.Physics;

namespace ACE.Server.Tests.Physics
{
    [TestClass]
    public class EnvCellTests
    {
        [TestMethod]
        public void BuildVisibleCells_IgnoresDuplicateIDs()
        {
            var engine = new PhysicsEngine(new ObjectMaint(), new SmartBox());
            engine.Server = false;

            uint cellId = 0x00010001;
            var envCell = new EnvCell();
            envCell.ID = cellId;
            envCell.VisibleCellIDs = new List<ushort> { 0x0200, 0x0200 };

            var landblock = new Landblock();
            landblock.ID = cellId | 0xFFFF;
            var visible = new EnvCell();
            visible.ID = (cellId & 0xFFFF0000) | 0x0200;
            landblock.LandCells.TryAdd(0x0200, visible);
            LScape.Landblocks.TryAdd(landblock.ID, landblock);

            try
            {
                envCell.build_visible_cells();
                Assert.AreEqual(1, envCell.VisibleCells.Count);
            }
            finally
            {
                LScape.Landblocks.TryRemove(landblock.ID, out _);
            }
        }
    }
}
