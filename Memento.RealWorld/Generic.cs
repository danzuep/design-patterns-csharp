using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Memento.Generic;

// Memento: stores a snapshot of the state
public sealed class Memento<T>
{
    private string _state;

    public T? State => JsonSerializer.Deserialize<T>(_state);

    public Memento(T state) => _state = JsonSerializer.Serialize(state);
}

// Originator: encapsulates the current state
public sealed class Originator<T> : INotifyPropertyChanged
{
    private T _state;

    public T State
    {
        get => _state;
        set
        {
            if (!Equals(_state, value))
            {
                _state = value;
                OnPropertyChanged();
                OnStateChanged?.Invoke(this, _state);
            }
        }
    }

    public Originator(T state) => _state = state;

    public Memento<T> Snapshot() => new(_state);

    public void Restore(Memento<T> memento)
    {
        State = memento.State;
    }

    public event EventHandler<T>? OnStateChanged;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// Caretaker: manages memento snapshots
public class Caretaker<T>
{
    private readonly ConcurrentStack<Memento<T>> _history = new();

    public void Save(Memento<T> memento)
    {
        _history.Push(memento);
    }

    public bool TryUndo(out Memento<T> memento)
    {
        return _history.TryPop(out memento!);
    }

    public void Clear()
    {
        while (_history.TryPop(out _)) { }
    }
}

public class StateManager<T> : INotifyPropertyChanged
{
    private readonly Originator<T> _originator;
    private readonly Caretaker<T> _caretaker;

    private readonly object _lock = new();

    public StateManager(Originator<T> originator, Caretaker<T> caretaker)
    {
        _originator = originator ?? throw new ArgumentNullException(nameof(originator));
        _caretaker = caretaker ?? throw new ArgumentNullException(nameof(caretaker));
        _originator.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(Originator<T>.State))
            {
                OnPropertyChanged(nameof(State));
            }
        };
        _originator.OnStateChanged += (s, state) => OnStateChanged?.Invoke(this, state);
    }

    public T State
    {
        get
        {
            lock (_lock)
            {
                return _originator.State;
            }
        }
    }

    public void Save()
    {
        lock (_lock)
        {
            _caretaker.Save(_originator.Snapshot());
        }
    }

    /// <summary>
    /// Updates the current state.
    /// If <paramref name="savePrevious"/> is true (default), saves current state before updating.
    /// </summary>
    public void Update(T state, bool savePrevious = true)
    {
        lock (_lock)
        {
            if (savePrevious)
            {
                _caretaker.Save(_originator.Snapshot());
            }

            _originator.State = state;
        }
    }

    public bool Undo()
    {
        lock (_lock)
        {
            if (_caretaker.TryUndo(out var previousState) && previousState != null)
            {
                _originator.Restore(previousState);
                return true;
            }
            return false;
        }
    }

    public void ClearHistory()
    {
        lock (_lock)
        {
            _caretaker.Clear();
        }
    }

    public event EventHandler<T>? OnStateChanged;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}