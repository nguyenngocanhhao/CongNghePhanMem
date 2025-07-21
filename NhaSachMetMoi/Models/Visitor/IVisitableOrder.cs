public interface IVisitableOrder
{
    void Accept(IOrderVisitor visitor);
}
