using MacroStaticAnalyzer.Business;
using NUnit.Framework;

namespace MacroStaticAnalyzer.UnitTests
{
    public class LexicalAnalyzerTest : AssertionHelper
    {
        private LexicalAnalyzer _lexer;

        [Test]
        public void When_read_then_read_correct_token1()
        {
            _lexer.Load(@"
			println ('[HAL]MixedMode.InputModeDD.SetPos');

			High_Level_Library_InputMode.InputModeDD_Set (pos);
		");

            Expect(_lexer.Read().ToString(), Is.EqualTo("println"));
            Expect(_lexer.Read().ToString(), Is.EqualTo("("));
            Expect(_lexer.Read().ToString(), Is.EqualTo("[HAL]MixedMode.InputModeDD.SetPos"));
            Expect(_lexer.Read().ToString(), Is.EqualTo(")"));
            Expect(_lexer.Read().ToString(), Is.EqualTo(";"));
            Expect(_lexer.Read().ToString(), Is.EqualTo("High_Level_Library_InputMode"));
            Expect(_lexer.Read().ToString(), Is.EqualTo("."));
            Expect(_lexer.Read().ToString(), Is.EqualTo("InputModeDD_Set"));
            Expect(_lexer.Read().ToString(), Is.EqualTo("("));
            Expect(_lexer.Read().ToString(), Is.EqualTo("pos"));
            Expect(_lexer.Read().ToString(), Is.EqualTo(")"));
            Expect(_lexer.Read().ToString(), Is.EqualTo(";"));
        }

        [Test]
        public void When_read_then_read_correct_token2()
        {
            _lexer.Load(@"
			println('[HAL]HlibInputMode.InputModeDD_Set');
			
			// In CXX, use direct pos from CXX
			int physicalPos = CxxPos;
			
			if(env:HiDriverWorkMode != HiDriverWorkMode.Fitting)
			{
				// In Hicoss, needs to Remap the pos to pos_wrt_basic_fcn using the Remapping function
				physicalPos = Mid_Level_Library_InputMode.InputModeDD_Remapping (CxxPos);
			}
			
			// sets input mode based on physical position
			High_Level_Library_InputMode.InputModeDD_SetAfterRemapping_with_Adaptive_features (physicalPos);
		");

            Expect(_lexer.ReadToEnd().Count, Is.EqualTo(38));

        }

        [SetUp]
        public void SetUp()
        {
            _lexer = new LexicalAnalyzer();
        }
    }
}
