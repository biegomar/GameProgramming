namespace Vectors.Test
{
    public class VectorTest
    {
        [Fact]
        public void VectorAddTest()
        {
            //arrange
            var vec1 = new Vector(10, 10, 10);
            var vec2 = new Vector(1, 2, 3);

            var expected = new Vector(11, 12, 13);

            //act
            var result = vec1 + vec2;

            //assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VectorSubTest()
        {
            //arrange
            var vec1 = new Vector(10, 10, 10);
            var vec2 = new Vector(1, 2, 3);

            var expected = new Vector(9, 8, 7);

            //act
            var result = vec1 - vec2;

            //assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VectorMultTest()
        {
            //arrange
            var vec1 = new Vector(10, 10, 10);
            var factor = 5;

            var expected = new Vector(50, 50, 50);

            //act
            var result = vec1 * factor;

            //assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VectorStaticDotProducTest()
        {
            //arrange
            var vec1 = new Vector(10, 10, 10);
            var vec2 = new Vector(3, 5, 2);

            var expected = 100;

            //act
            var result = Vector.DotProduct(vec1, vec2);

            //assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VectorDotProductTest()
        {
            //arrange
            var vec1 = new Vector(10, 10, 10);
            var vec2 = new Vector(3, 5, 2);

            var expected = 100;

            //act
            var result = vec1.DotProduct(vec2);

            //assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VectorScalarMultTest()
        {
            //arrange
            var vec1 = new Vector(10, 10, 10);
            var vec2 = new Vector(3, 5, 2);

            var expected = 100;

            //act
            var result = vec1 * vec2;

            //assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VectorStaticCrossProductTest()
        {
            //arrange
            var vec1 = new Vector(11, 12, 13);
            var vec2 = new Vector(3, 5, 2);

            var expected = new Vector(-41, 17, 19);

            //act
            var result = Vector.CrossProduct(vec1, vec2);

            //assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VectorCrossProductTest()
        {
            //arrange
            var vec1 = new Vector(11, 12, 13);
            var vec2 = new Vector(3, 5, 2);

            var expected = new Vector(-41, 17, 19);

            //act
            var result = vec1.CrossProduct(vec2);

            //assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VectorStaticDistanceTest()
        {
            //arrange
            var vec1 = new Vector(11, 12, 13);
            var vec2 = new Vector(3, 5, 2);

            var expected = 15.297f;

            //act
            var result = Vector.Distance(vec1, vec2);

            //assert
            Assert.Equal(expected, result, 0.001f);
        }

        [Fact]
        public void VectorDistanceTest()
        {
            //arrange
            var vec1 = new Vector(11, 12, 13);
            var vec2 = new Vector(3, 5, 2);

            var expected = 15.297f;

            //act
            var result = vec1.Distance(vec2);

            //assert
            Assert.Equal(expected, result, 0.001f);
        }

        [Fact]
        public void VectorSquaredLengthTest()
        {
            //arrange
            var vec1 = new Vector(10, 10, 10);            

            var expected = 300;

            //act
            var result = vec1.SquaredLength();

            //assert
            Assert.Equal(expected, result, 0.001f);
        }

        [Fact]
        public void VectorLengthTest()
        {
            //arrange
            var vec1 = new Vector(10, 10, 10);

            var expected = 17.32f;

            //act
            var result = vec1.Length();

            //assert
            Assert.Equal(expected, result, 0.001f);
        }
    }
}