namespace CharacterControllerFactory {
    public interface IPredicate {
        // A predicate is a function that tests a condition and then returns a bool
        // We define an interface for this because we might have some complex predicates 
        // to evaluate when determining if we can perform a state transtiion 
        bool Evaluate();
    }
}
