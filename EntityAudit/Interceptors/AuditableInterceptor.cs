using EntityAudit.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EntityAudit.Interceptors;

public class AuditableInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;

    public AuditableInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken
    )
    {
        if (eventData.Context is not null)
        {
            ApplyAuditFields(eventData.Context);
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);

    }

    private void ApplyAuditFields(DbContext context)
    {
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        var currentUser = _currentUserService.GetCurrentUser();
        var currentTime = DateTime.UtcNow;
        foreach (var entry in entries)
        {
            var auditable = (IAuditable)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                auditable.CreatedAt = currentTime;
                auditable.CreatedBy = currentUser;
            }
            else
            {
                auditable.UpdatedAt = currentTime;
                auditable.UpdatedBy = currentUser;
            }
            
        }
        
    }
    
    
    
}
