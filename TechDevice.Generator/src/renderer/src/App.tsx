import React from 'react'
import GeneratorKnob from './components/GeneratorKnob'
import './App.css'

export type AppState = {
    readonly voltage: number;
    readonly amperage: number; 
}
class App extends React.Component<{}, AppState> {
    public constructor(props: {}) {
        super(props);
        this.state = { voltage: 0, amperage: 0 };
    }
    private generatorChanged = (state: AppState): void => {
        window.api.invoke.getData(state);
        this.setState({
            amperage: state.amperage,
            voltage: state.voltage, 
        });
    };
    public override render(): React.ReactElement {
        return (
            <div className='main-content'>
                <div className='device-items'>
                    <GeneratorKnob componentName={'Сила тока'} 
                        onValueChange={(value) => this.generatorChanged({
                            amperage: value,
                            voltage: this.state.voltage
                        })}
                        labelFormater={(value: string) => `${value} А`}/>
                </div>
                <div className='device-items'>
                    <GeneratorKnob componentName={'Напряжение'} 
                        onValueChange={(value) => this.generatorChanged({
                            amperage: this.state.amperage,
                            voltage: value
                        })}
                        labelFormater={(value: string) => `${value} В`}/>
                </div>
            </div>
        );
    }
}
export default App;
