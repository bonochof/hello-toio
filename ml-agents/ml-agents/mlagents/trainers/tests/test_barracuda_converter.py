import os
import tempfile
import pytest

import mlagents.trainers.tf.tensorflow_to_barracuda as tf2bc
from mlagents.trainers.tests.test_nn_policy import create_policy_mock
from mlagents.trainers.settings import TrainerSettings
from mlagents.tf_utils import tf
from mlagents.model_serialization import SerializationSettings


def test_barracuda_converter():
    path_prefix = os.path.dirname(os.path.abspath(__file__))
    tmpfile = os.path.join(
        tempfile._get_default_tempdir(), next(tempfile._get_candidate_names()) + ".nn"
    )

    # make sure there are no left-over files
    if os.path.isfile(tmpfile):
        os.remove(tmpfile)

    tf2bc.convert(path_prefix + "/BasicLearning.pb", tmpfile)

    # test if file exists after conversion
    assert os.path.isfile(tmpfile)
    # currently converter produces small output file even if input file is empty
    # 100 bytes is high enough to prove that conversion was successful
    assert os.path.getsize(tmpfile) > 100

    # cleanup
    os.remove(tmpfile)


@pytest.mark.parametrize("discrete", [True, False], ids=["discrete", "continuous"])
@pytest.mark.parametrize("visual", [True, False], ids=["visual", "vector"])
@pytest.mark.parametrize("rnn", [True, False], ids=["rnn", "no_rnn"])
def test_policy_conversion(tmpdir, rnn, visual, discrete):
    tf.reset_default_graph()
    dummy_config = TrainerSettings()
    policy = create_policy_mock(
        dummy_config,
        use_rnn=rnn,
        model_path=os.path.join(tmpdir, "test"),
        use_discrete=discrete,
        use_visual=visual,
    )
    settings = SerializationSettings(policy.model_path, "MockBrain")
    checkpoint_path = f"{tmpdir}/MockBrain-1"
    policy.checkpoint(checkpoint_path, settings)

    # These checks taken from test_barracuda_converter
    assert os.path.isfile(checkpoint_path + ".nn")
    assert os.path.getsize(checkpoint_path + ".nn") > 100
