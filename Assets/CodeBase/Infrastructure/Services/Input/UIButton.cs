namespace CodeBase.Infrastructure.Services.Input
{
    public interface UIButton
    {
        bool IsPressed { get; set; }
        void ButtonDown();
        void ButtonUp();
    }
}