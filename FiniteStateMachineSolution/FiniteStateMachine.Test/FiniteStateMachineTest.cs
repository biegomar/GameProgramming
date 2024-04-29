namespace FiniteStateMachine.Test;

public class FiniteStateMachineTest
{
    [Fact]
    public void Test1()
    {
        //arrange
        var idleState = new State();
        var patrolState = new State();
        var attackState = new State(); 
            
        bool shouldChangeToPatrolState = true;
        bool shouldChangeToAttackState = true;

        var transitToPatrolState = new Transition(() => shouldChangeToPatrolState, patrolState);
        var transitToAttackState = new Transition(() => shouldChangeToAttackState, attackState);
        idleState.AddTransition(transitToPatrolState);
        idleState.AddTransition(transitToAttackState);
        
        var sut = new FiniteStateMachine(idleState);
        sut.AddState(patrolState);
        
        //act
        sut.StartMachine();
        sut.UpdateMachine();
        
        //assert
        Assert.Equal(patrolState, sut.ActiveState);
    }
}